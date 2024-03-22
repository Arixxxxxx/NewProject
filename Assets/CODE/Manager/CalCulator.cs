using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CalCulator : MonoBehaviour
{
    public static CalCulator inst;
    StringBuilder sb = new StringBuilder();
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 스트링끼리의 덧셈 함수
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public string DigidPlus(string a, string b)
    {
        sb.Clear();
        int maxLength = Mathf.Max(a.Length, b.Length);
        string A = a.PadLeft(maxLength, '0');
        string B = b.PadLeft(maxLength, '0');
        int carry = 0;

        // 끝에서부터 계산 시작
        for (int index = maxLength - 1; index >= 0; index--)
        {
            int sum = int.Parse(A[index].ToString()) + int.Parse(B[index].ToString()) + carry;
            carry = sum / 10; // 다음 자리로 넘길 올림값
            sb.Insert(0, sum % 10); // 현재 자릿수 값을 결과에 추가
        }

        if (carry > 0) // 마지막 올림 처리
        {
            sb.Insert(0, carry);
        }

        return sb.ToString();
    }



    /// <summary>
    /// 스트링끼리의 뺄셈 함수
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public string DigidMinus(string a, string b)
    {
        sb.Clear();
        string result = string.Empty;
        int maxLength = Mathf.Max(a.Length, b.Length);
        string A = a.PadLeft(maxLength, '0');
        string B = b.PadLeft(maxLength, '0');
        int carry = 0;

        if (int.Parse(A[0].ToString()) < int.Parse(B[0].ToString())) // 만약 결과가 음수로 될시 죽음처리
        {
            return "Dead";
        }

        for (int index = maxLength - 1; index >= 0; index--)
        {
            int minus = (int.Parse(A[index].ToString()) - carry) - int.Parse(B[index].ToString());
            if (minus < 0)
            {
                carry = 1;
                minus += 10;
            }
            else
            {
                carry = 0;
            }
            sb.Insert(0, minus);
        }

        result = sb.ToString().TrimStart('0');
        return result == string.Empty ? "Dead" : result; // 결과가 빈 문자열인 경우 0이니깐 "Dead"을 반환
    }





    /// <summary>
    /// 최종 숫자 + 문자 변환 함수
    /// </summary>
    /// <param name="inputValue"> 계산된 정수들어있는 스트링 집어넣으면됨 </param>
    /// <returns></returns>
    public string StringFourDigitChanger(string inputValue)
    {
        int index = 0;
        int digitLength = inputValue.Length;
        string digit = inputValue;
        string word = "";

        // 숫자 가져오는 부분 (뒷자리 3개씩 지우면서 인덱스값 올려줌)
        while (digitLength > 3)
        {
            index++;
            digitLength -= 3; // 뒷자리 3개씩 지움
            digit = digit.Substring(0, digitLength); // 지우고앞자리만 가져옴
        }

        // 알파벳 가져오는 부분
        if (index > 0)
        {
            word += (char)(64 + index);
        }

        return digit + word;
    }
}
