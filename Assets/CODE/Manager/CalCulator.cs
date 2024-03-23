using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// ��Ʈ�������� ���� �Լ�
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

        // ���������� ��� ����
        for (int index = maxLength - 1; index >= 0; index--)
        {
            int sum = int.Parse(A[index].ToString()) + int.Parse(B[index].ToString()) + carry;
            carry = sum / 10; // ���� �ڸ��� �ѱ� �ø���
            sb.Insert(0, sum % 10); // ���� �ڸ��� ���� ����� �߰�
        }

        if (carry > 0) // ������ �ø� ó��
        {
            sb.Insert(0, carry);
        }

        return sb.ToString();
    }



    /// <summary>
    /// ��Ʈ�������� ���� �Լ�
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

        if (int.Parse(A[0].ToString()) < int.Parse(B[0].ToString())) // ���� ����� ������ �ɽ� ����ó��
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
        return result == string.Empty ? "Dead" : result; // ����� �� ���ڿ��� ��� 0�̴ϱ� "Dead"�� ��ȯ
    }


    /// <summary>
    /// %���� : StringFourDigitChanger �Լ� ���� ���� ���ڿ� + % ����־����
    /// </summary>
    /// <param name="a"> Ex : 555A </param>
    /// <param name="percent"> 5% => 5 </param>
    /// <returns></returns>
    public string DigitMultiply(string a, int percent)
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

            int numTemp = int.Parse(string.Join("", a.Where(x => char.IsDigit(x)).Select(x => x).ToArray())); //���ڲ���
            string letter = new string(a.Where(x => char.IsLetter(x)).Select(x => x).ToArray());  //���� ����

            int calcul = (int)Mathf.Floor(numTemp * (1 + 0.01f * percent));

            string numberPart = calcul.ToString(); // ���� ����ȭ
            int digitLength = numberPart.Length; //���� ����
            int index = 0;

            while (digitLength > 3)
            {
                index++;
                digitLength -= 3; // ���ڸ� 3���� ����
                numberPart = numberPart.Substring(0, digitLength); // ��������ڸ��� ������
            }

            sb.Append(numberPart + (char)((int)letter[0] + index));

            return sb.ToString();
        }

        return string.Empty;
    }



    /// <summary>
    /// ���� ���� + ���� ��ȯ �Լ�
    /// </summary>
    /// <param name="inputValue"> ���� ��������ִ� ��Ʈ�� ���������� </param>
    /// <returns></returns>
    public string StringFourDigitChanger(string inputValue)
    {
        int index = 0;
        int digitLength = inputValue.Length;
        string digit = inputValue;
        string word = "";

        // ���� �������� �κ� (���ڸ� 3���� ����鼭 �ε����� �÷���)
        while (digitLength > 3)
        {
            index++;
            digitLength -= 3; // ���ڸ� 3���� ����
            digit = digit.Substring(0, digitLength); // ��������ڸ��� ������
        }

        // ���ĺ� �������� �κ�
        if (index > 0)
        {
            word += (char)(64 + index);
        }

        return digit + word;
    }
}