using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using UnityEngine;

public class CalCulator : MonoBehaviour
{
    public static CalCulator inst;
    StringBuilder sb = new StringBuilder();
    BigInteger forCalculatorA = new BigInteger();
    BigInteger forCalculatorB = new BigInteger();
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
        
        string tempA = new string(a.Where( x=> char.IsDigit(x) ).ToArray());
        string tempB = new string(b.Where( x=> char.IsDigit(x) ).ToArray());

        int maxLength = Mathf.Max(a.Length, b.Length);
        string A = tempA.PadLeft(maxLength, '0');
        string B = tempB.PadLeft(maxLength, '0');
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
    /// %계산기 : StringFourDigitChanger 함수 들어갔다 나온 문자열 + % 집어넣어야함
    /// </summary>
    /// <param name="a"> Ex : 555A </param>
    /// <param name="percent"> 5% => 5 </param>
    /// <returns></returns>
    public string DigitPercentMultiply(string a, int percent)
    {
        sb.Clear();
                               
        int wordIndex = a.Count(x => char.IsLetter(x));

        if (wordIndex == 0)
        {
            float sum = int.Parse(a) * (1 + 0.01f * percent);
            int temp = (int)Mathf.Floor(sum);
            return StringFourDigitChanger(temp.ToString());

        }
        else if (wordIndex > 0)
        {

            int numTemp = int.Parse(string.Join("", a.Where(x => char.IsDigit(x)).Select(x => x).ToArray())); //숫자꺼냄
            string letter = new string(a.Where(x => char.IsLetter(x)).Select(x => x).ToArray());  //문자 꺼냄

            int calcul = (int)Mathf.Floor(numTemp * (1 + 0.01f * percent));

            string numberPart = calcul.ToString(); // 숫자 문자화
            int digitLength = numberPart.Length; //문자 길이
            int index = 0;

            while (digitLength > 3)
            {
                index++;
                digitLength -= 3; // 뒷자리 3개씩 지움
                numberPart = numberPart.Substring(0, digitLength); // 지우고앞자리만 가져옴
            }

            sb.Append(numberPart + (char)((int)letter[0] + index));

            return sb.ToString();
        }

        return string.Empty;
    }


    /// <summary>
    /// 공격력 스트링으로 변환하여 리턴
    /// </summary>
    /// <returns></returns>
    public string Get_ATKtoString()
    {
        return UIManager.Instance.TotalAtk.ToString();
    }

    public string ToBigIntigerFromString(BigInteger value)
    {
        return value.ToString();
    }

    BigInteger hpA = new BigInteger();
    public string EnemyHpSetup()
    {
        hpA = 200; // 기존 2 임시로 200
        hpA = BigInteger.Pow(hpA, GameStatus.inst.AccumlateFloor);

        return hpA.ToString();
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
        }

        digitLength = Mathf.Min(digitLength, digit.Length);
        digit = digit.Substring(0, digitLength); // 지우고앞자리만 가져옴

        // 알파벳 가져오는 부분
        if (index > 0)
        {
            word += (char)(64 + index);
        }

        return digit + word;
    }

    /// <summary>
    /// 문자 제외 앞자리 숫자만 리턴
    /// </summary>
    /// <param name="inputValue"></param>
    /// <returns></returns>
    public string OlnyDigitChanger(string inputValue)
    {
        int index = 0;
        int digitLength = inputValue.Length;
        string digit = new string(inputValue.Where( x => char.IsDigit(x) == true).ToArray());

        while (digitLength > 3)
        {
            index++;
            digitLength -= 3; // 뒷자리 3개씩 지움
        }

        digitLength = Mathf.Min(digitLength, digit.Length);
        digit = digit.Substring(0, digitLength);

        return digit;
    }

    /// <summary>
    /// float에int 제곱 결과값이 biginteger일 때 사용
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pow"></param>
    /// <returns></returns>
    public BigInteger CalculatePow(float value, int pow)
    {
        string strValue = value.ToString();
        int count = strValue.Length;
        int pointNum = 0;
        char point = '.';
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (Equals(strValue[iNum], point))
            {
                pointNum = iNum;
                break;
            }
        }
        if (pointNum != 0)
        {
            int pracCount = strValue.Length - pointNum - 1;
            float powpracCount = Mathf.Pow(10, pracCount);
            BigInteger intvalue = (BigInteger)(value * powpracCount);
            BigInteger powResult = BigInteger.Pow(intvalue, pow);
            BigInteger Result = BigInteger.Divide(powResult, BigInteger.Pow((BigInteger)powpracCount, pow));
            return Result;
        }
        else
        {
            return BigInteger.Parse(strValue);
        }
    }

    /// <summary>
    /// int에 float만큼 제곱 결과값이 Biginteger일 때 사용
    /// </summary>
    /// <param name="value"></param>
    /// <param name="pow"></param>
    /// <returns></returns>
    public BigInteger CalculatePow(int value, float pow)
    {
        int intpowNum = (int)Mathf.Floor(pow);
        float pracpowNum = pow - intpowNum;
        BigInteger temp = BigInteger.Pow(value, intpowNum);
        float temp2 = Mathf.Pow(value, pracpowNum);
        BigInteger resultPowNum = BigInteger.Multiply(temp, (BigInteger)temp2);
        return resultPowNum;
    }

    /// <summary>
    /// Biginteger에 float곱 계산
    /// </summary>
    /// <param name="Ivalue"></param>
    /// <param name="fvalue"></param>
    /// <returns></returns>
    public BigInteger MultiplyBigIntegerAndfloat(BigInteger Ivalue, float fvalue)
    {
        string strValue = fvalue.ToString();
        int count = strValue.Length;
        int pointNum = 0;
        char point = '.';
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (Equals(strValue[iNum], point))
            {
                pointNum = iNum;
                break;
            }
        }
        if (pointNum != 0)
        {
            int pracCount = strValue.Length - pointNum - 1;
            float powpracCount = Mathf.Pow(10, pracCount);
            BigInteger intvalue = (BigInteger)(fvalue * powpracCount);
            BigInteger result = BigInteger.Multiply(Ivalue, intvalue);
            result = BigInteger.Divide(result, (BigInteger)powpracCount);
            return result;
        }
        else
        {
            BigInteger result = BigInteger.Multiply(Ivalue, (BigInteger)fvalue);
            return result;
        }
    }


    /// <summary>
    /// Biginteger를 float으로 나누기
    /// </summary>
    /// <param name="ivalue"></param>
    /// <param name="fvalue"></param>
    /// <returns></returns>
    public BigInteger DevideBigIntegerAndfloat(BigInteger ivalue, float fvalue)//BigInteger / float 계산기
    {
        string strValue = fvalue.ToString();
        int count = strValue.Length;
        int pointNum = 0;
        char point = '.';
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (Equals(strValue[iNum], point))
            {
                pointNum = iNum;
                break;
            }
        }
        if (pointNum != 0)
        {
            int pracCount = strValue.Length - pointNum - 1;
            float powpracCount = Mathf.Pow(10, pracCount);
            BigInteger intvalue = (BigInteger)(fvalue * powpracCount);
            BigInteger result = BigInteger.Divide(ivalue, intvalue);
            result = BigInteger.Multiply(result, (BigInteger)powpracCount);
            return result;
        }
        else
        {
            BigInteger result = BigInteger.Divide(ivalue, (BigInteger)fvalue);
            return result;
        }
    }
    

    /// <summary>
    /// 플레이어 치명타적중시 치명타피해량 합산하여 리턴하는 함수
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public string PlayerCriDMGCalculator(string playerDMG)
    {
        sb.Clear();
        forCalculatorA = BigInteger.Parse(playerDMG);
        double critMultiplier = 2 + (GameStatus.inst.CriticalPower / 100.0);
        forCalculatorA = BigInteger.Multiply(forCalculatorA, new BigInteger(critMultiplier));
        sb.Append(forCalculatorA);

        return sb.ToString();
    }


    /// <summary>
    /// string * int => return string
    /// </summary>
    /// <param name="a"> string / int </param>
    /// <returns></returns>
    public string StringAndIntMultiPly(string value, int multiplyValue)
    {
        sb.Clear();
        return sb.Append(BigInteger.Multiply(BigInteger.Parse(value), multiplyValue)).ToString();

    }



    /// <summary>
    /// 체력 환산용
    /// </summary>
    /// <param name="cur"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public float ForImageFillAmout(string cur, string max) 
    {
        float A = float.Parse(OlnyDigitChanger(cur));
        float B = float.Parse(OlnyDigitChanger(max));

        Debug.Log($"{A} / {B}  ,  {A/B}");

        return A/B;

        //forCalculatorA = BigInteger.Parse(new string(cur.Where( x => char.IsDigit(x)).ToArray()));
        //forCalculatorB = BigInteger.Parse(new string(max.Where(x => char.IsDigit(x)).ToArray()));
    }






}
