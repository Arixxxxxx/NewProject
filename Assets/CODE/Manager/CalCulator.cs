using System.Linq;
using System.Numerics;
using System.Text;
using UnityEngine;

public class CalCulator : MonoBehaviour
{
    public static CalCulator inst;
    StringBuilder sb = new StringBuilder();
    BigInteger forCalculatorA = new BigInteger();




    private void Awake()
    {
        if (inst == null)
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
    public string DigidMinus(string a, string b, bool areYouEnemy)
    {
        sb.Clear();
        string result = string.Empty;

        string tempA = new string(a.Where(x => char.IsDigit(x)).ToArray());
        string tempB = new string(b.Where(x => char.IsDigit(x)).ToArray());

        int maxLength = Mathf.Max(a.Length, b.Length);
        string A = tempA.PadLeft(maxLength, '0');
        string B = tempB.PadLeft(maxLength, '0');
        int carry = 0;

        if (areYouEnemy == true && int.Parse(A[0].ToString()) < int.Parse(B[0].ToString())) // ���� ����� ������ �ɽ� ����ó��
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

        if (result == string.Empty && areYouEnemy == true)
        {
            result = "Dead";
        }
        else if (result == string.Empty && areYouEnemy == false)
        {
            result = "0";
        }

        return result; // ����� �� ���ڿ��� ��� 0�̴ϱ� "Dead"�� ��ȯ
    }



    /// <summary>
    /// %���� : StringFourDigitChanger �Լ� ���� ���� ���ڿ� + % ����־����
    /// </summary>
    /// <param name="a"> Ex : 555A </param>
    /// <param name="percent"> 5% => 5 </param>
    /// <returns></returns>
    public string DigitPercentMultiply(string a, int percent)
    {
        // �Է� ���ڿ��� BigInteger�� �Ľ�
        BigInteger original = BigInteger.Parse(a);

        // percent�� ����Ͽ� ������ ������ BigInteger�� ���
        // ��: percent�� 5���, totalPercent�� 105% ��, 1.05�� �Ǿ����
        // BigInteger�� �Ҽ����� �������� �����Ƿ�, �м��� ó��
        BigInteger numerator = 100 + percent;
        BigInteger denominator = 100;

        // ��� ���: (original * numerator) / denominator
        // ���ڿ� original�� numerator�� ���ϰ�, �и�� denominator�� ���
        BigInteger result = (original * numerator) / denominator;

        return result.ToString();
    }



    /// <summary>
    /// �����ݷ� �ջ��Ͽ� ��Ʈ������ ��ȯ�Ͽ� ����
    /// </summary>
    /// <returns></returns>
    public string Get_CurPlayerATK()
    {
        string result = string.Empty;

        // 15�� ������ üũ
        result = DigidPlus(GameStatus.inst.TotalAtk.ToString(), GameStatus.inst.BuffAddATK);

        //// 5�� ������ üũ (ȭ�鱤��)
        result =  DigidPlus(result, GameStatus.inst.BuffAddAdATK);

        //// �ʽ��� ����
        result = DigidPlus(result, GameStatus.inst.NewbieATKBuffValue);

        // �� ������ üũ
        result = DigidPlus(result, GameStatus.inst.AddPetAtkBuff);

        /// ���⵵�� ī��Ʈ��ŭ % �ջ�
        result = DigitPercentMultiply(result, DogamManager.inst.weaponDogamGetCount);

        // �ϴ� UI �ؽ�Ʈ ǥ��
        UIManager.Instance.SetAtkText(inst.StringFourDigitAddFloatChanger(result));

        return result;
    }

    public string ToBigIntigerFromString(BigInteger value)
    {
        return value.ToString();
    }

    BigInteger hpA = new BigInteger();
    public string EnemyHpSetup()
    {
        hpA = 2; // ���� 2 �ӽ÷� 200
        hpA = BigInteger.Pow(hpA, GameStatus.inst.AccumlateFloor);

        return hpA.ToString();
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
        }

        digitLength = Mathf.Min(digitLength, digit.Length);
        digit = digit.Substring(0, digitLength); // �������ڸ��� ������

        // ���ĺ� �������� �κ�
        if (index > 0)
        {
            //word += (char)(64 + index); �ƽ�Ű�ڵ� �������� ���� A -> ZZ
            word = MakeBigDigitWord(index);
        }

        return digit + word;
    }



    /// <summary>
    /// �Ҽ��� ���� ���ڹ��� ��ȯ�� ( Ex : 1.1A , 232.1B �� )
    /// </summary>
    /// <param name="inputValue"></param>
    /// <returns></returns>
    public string StringFourDigitAddFloatChanger(string inputValue)
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
        }

        digitLength = Mathf.Min(digitLength, digit.Length);
        digit = digit.Substring(0, digitLength); // �������ڸ��� ������

        if (inputValue.Length > 3) //�Ҽ��� �ٿ���
        {
            digit += ".";
            digit += new string(inputValue.Skip(digitLength).Take(1).ToArray()); // �Ҽ��� ���ڸ����� ������
        }

        // ���ĺ� �������� �κ�
        if (index > 0)
        {
            //word += (char)(64 + index); �ƽ�Ű�ڵ� �������� ���� A -> ZZ
            word = MakeBigDigitWord(index);
        }

        return digit + word;
    }



    /// <summary>
    /// ���ڵڿ� ���ڷ� �ջ����ִ� �Լ�
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private string MakeBigDigitWord(int index)
    {
        string result = string.Empty;
        while (index > 0)
        {
            index--; // 0 ������� ����
            int remainder = index % 26;
            char letter = (char)('A' + remainder);
            result = letter + result;
            index = (index / 26); // ��Ȯ�� ������
        }
        return result;
    }
    /// <summary>
    /// ���ڵ��� ���ڸ� ���ڷ� �ջ�����ִ� �Լ�
    /// </summary>
    /// <param name="_text"></param>
    /// <returns></returns>
    public string ConvertChartoIndex(string _text)
    {
        char firalp = _text[_text.Length - 1];
        char secAlp = _text[_text.Length - 2];
        int firNum = firalp - 64;
        int secNum = secAlp - 64;
        int firindex = 0;
        int secindex = 0;
        if (firNum >= 1 && firNum <= 26)
        {
            firindex = firNum * 3;
            _text = _text.Remove(_text.Length - 1);
        }
        if (secNum >= 1 && secNum <= 26)
        {
            secindex = secNum * 26;
            _text = _text.Remove(_text.Length - 1);
        }
        string result = _text;
        int count = firindex + secindex;
        for (int iNum = 0; iNum < count; iNum++)
        {
            result += '0';
        }

        return result;
    }
    /// <summary>
    /// ���� ���� ���ڸ� ���ڸ� ����
    /// </summary>
    /// <param name="inputValue"></param>
    /// <returns></returns>
    public string OlnyDigitChanger(string inputValue)
    {
        int index = 0;
        int digitLength = inputValue.Length;
        string digit = new string(inputValue.Where(x => char.IsDigit(x) == true).ToArray());

        while (digitLength > 3)
        {
            index++;
            digitLength -= 3; // ���ڸ� 3���� ����
        }

        digitLength = Mathf.Min(digitLength, digit.Length);
        digit = digit.Substring(0, digitLength);

        return digit;
    }

    /// <summary>
    /// float��int ���� ������� biginteger�� �� ���
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
    /// int�� float��ŭ ���� ������� Biginteger�� �� ���
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
    /// Biginteger�� float�� ���
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
    /// Biginteger�� float���� ������
    /// </summary>
    /// <param name="ivalue"></param>
    /// <param name="fvalue"></param>
    /// <returns></returns>
    public BigInteger DevideBigIntegerAndfloat(BigInteger ivalue, float fvalue)//BigInteger / float ����
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
    /// �÷��̾� ġ��Ÿ���߽� ġ��Ÿ���ط� �ջ��Ͽ� �����ϴ� �Լ�
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
    /// 
    /// </summary>
    /// <param name="cur"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public float StringAndStringDivideReturnFloat(string cur, string max, int precision)
    {
        if (cur == "Dead") { cur = "0"; }


        BigInteger bigA = BigInteger.Parse(cur);
        BigInteger bigB = BigInteger.Parse(max);

        // ���ڸ� Ȯ���Ͽ� �Ҽ��� ���� ���е� Ȯ��
        BigInteger scale = BigInteger.Pow(10, precision);
        BigInteger expandedA = bigA * scale;

        // Ȯ��� ���ڸ� �и�� ����
        BigInteger result = expandedA / bigB;

        // ����� ���ڿ��� ��ȯ
        string resultString = result.ToString();

        // ��� ���ڿ��� �Ҽ��� �߰�
        if (resultString.Length <= precision)
        {
            // ����� 1���� ���� ���, ���� 0�� �߰�
            resultString = resultString.PadLeft(precision + 1, '0');
        }

        // ������ ��ġ�� �Ҽ��� ����
        string finalResult = resultString.Insert(resultString.Length - precision, ".");

        // �Ҽ��� ���� ���ʿ��� 0 ���� (�ɼ�)
        finalResult = finalResult.TrimEnd('0').TrimEnd('.');
        return float.Parse(finalResult);

    }
}
