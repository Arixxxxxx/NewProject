using System;
using System.Linq;
using System.Numerics;
using System.Text;
using UnityEngine;

public class CalCulator : MonoBehaviour
{
    public static CalCulator inst;
    StringBuilder sb = new StringBuilder();

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
    /// 
    /// </summary>
    /// <param name="a"> A ��</param>
    /// <param name="b"> B ��</param>
    /// <param name="areYouEnemy"> ���� ���Ͷ�� true �� �Ű����� �Է� / �Ϲ� ����̶�� false</param>
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
    /// String - String = return String (Bigintiger Type Calcluator)
    /// </summary>
    /// <param name="a">string data</param>
    /// <param name="b">string data</param>
    /// <returns></returns>
    public string BigIntigerMinus(string a, string b)
    {
        string result = string.Empty;

        BigInteger enemyHP = BigInteger.Parse(a);
        BigInteger DMG = BigInteger.Parse(b);

        BigInteger curHP = enemyHP - DMG;


        if (curHP <= 0)
        {
            result = "0";
        }
        else if (curHP > 0)
        {
            result = curHP.ToString();
        }

        return result;
    }

    /// <summary>
    /// %���� : StringFourDigitChanger �Լ� ���� ���� ���ڿ� + % ����־����
    /// </summary>
    /// <param name="a"> Ex : 555A </param>
    /// <param name="percent"> 5% => 5 </param>
    /// <returns></returns>
    public string DigitAndIntPercentMultiply(string a, int percent)
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

    public string DigitAndFloatPercentMultiply(string a, float percent)
    {
        // �Է� ���ڿ��� BigInteger�� �Ľ�
        BigInteger original = BigInteger.Parse(a);

        // percent�� ����Ͽ� ������ ������ BigInteger�� ���
        // ��: percent�� 5.5���, totalPercent�� 105.5% ��, 1.055�� �Ǿ�� ��
        // BigInteger�� �Ҽ����� �������� �����Ƿ�, �м��� ó��
        BigInteger numerator = (BigInteger)((100 + percent) * 1000); // �Ҽ��� 3�ڸ����� ���
        BigInteger denominator = 100000; // 100 * 1000

        // ��� ���: (original * numerator) / denominator
        // ���ڿ� original�� numerator�� ���ϰ�, �и�� denominator�� ���
        BigInteger result = (original * numerator) / denominator;

        return result.ToString();
    }




    int dmgMultiplyer = 0;
    float percent = 0;
    
    /// <summary>
    /// �����ݷ� �ջ��Ͽ� ��Ʈ������ ��ȯ�Ͽ� ����
    /// </summary>
    /// <returns></returns>
    public string Get_CurPlayerATK()
    {
        string originATK = GameStatus.inst.TotalAtk.ToString();
        dmgMultiplyer = 0;

        // 2�� ���� üũ
        if (BuffContoller.inst.BuffActiveCheck(0) == true)
        {
            dmgMultiplyer += 2;
        }

        //// 3�� ���� üũ
        //result = DigidPlus(result, GameStatus.inst.BuffAddAdATK);
        if (BuffContoller.inst.BuffActiveCheck(3) == true)
        {
            dmgMultiplyer += 3;
        }

        ////// �ʽ��� ���� 2��
        if (BuffContoller.inst.newbieBuffActiveCheck == true)
        {
            dmgMultiplyer += 2;
        }

        string buffDMG = string.Empty;

        if (dmgMultiplyer != 0)
        {
            buffDMG = StringAndIntMultiPly(originATK, dmgMultiplyer);
        }
        else
        {
            buffDMG = originATK;
        }


        percent = 0;

        //// �� ������ üũ { ������ 10% ����}
        if (GameStatus.inst.AddPetAtkBuff != "0")
        {
            percent += GameStatus.inst.Pet0_Lv * 10;
        }

        // ���� �Ϲݰ��ݷ� ���
        if (DogamManager.inst.Get_DogamATKBonus() != 0)
        {
            percent += DogamManager.inst.Get_DogamATKBonus();
        }

        // ���� �Ϲݰ��ݷ� ���
        if (GameStatus.inst.GetAryRelicLv(0) != 0)
        {
            percent = GameStatus.inst.RelicDefaultvalue(0) * GameStatus.inst.GetAryRelicLv(0);
        }

        if(percent != 0)
        {
            buffDMG = DigitAndFloatPercentMultiply(buffDMG, percent);
        }

        return buffDMG;
    }

    public string ToBigIntigerFromString(BigInteger value)
    {
        return value.ToString();
    }



    public BigInteger baseHP = new BigInteger(500);  // �⺻ ü��
    public double hpIncreaseFactor = 1.23;  // ü�� ���� ��� (�뷱�� ����)

    public BigInteger upBaseHP = new BigInteger(8888);  // �⺻ ü��
    public double higherHpIncreaseFactor = 1.52;  // �������� 150 ������ ü�� ���� ���

    // ���� ü�� �ʱ�ȭ
    public string EnemyHpSetup()
    {
        if (GameStatus.inst == null)
        {
            return null;
        }

        int stage = GameStatus.inst.AccumlateFloor + 1;
        BigInteger health;

        if (stage < 150)
        {
            // 150 ���������� ���� ü�� ���� ����
            BigInteger factor = new BigInteger(hpIncreaseFactor * 1000);
            BigInteger stageCubed = new BigInteger(stage) * stage * stage;
            health = baseHP + (factor * stageCubed / 1000);
        }
        else
        {
            // 150 ���ĺ��ʹ� �� ū ü�� ���� ����
            BigInteger factor = new BigInteger(higherHpIncreaseFactor * 1000);
            BigInteger stageCubed = new BigInteger(stage) * stage * stage;
            health = upBaseHP + (factor * stageCubed / 1000);
        }

        return health.ToString();
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
        int pointindex = -1;
        int count = _text.Length;
        for (int iNum = 0; iNum < count; iNum++)
        {
            if (Equals(_text[iNum], '.'))
            {
                pointindex = iNum;
                break;
            }

        }

        if (_text.Length == 1)
        {
            return _text;
        }
        else if (pointindex == -1)
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
            count = firindex + secindex;
            for (int iNum = 0; iNum < count; iNum++)
            {
                result += '0';
            }

            return result;
        }
        else
        {
            _text = _text.Remove(pointindex, 1);
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
            count = firindex + secindex - 1;
            for (int iNum = 0; iNum < count; iNum++)
            {
                result += '0';
            }

            return result;
        }
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
        if (pointNum != 0)//������ �ƴҰ��
        {
            int pracCount = strValue.Length - pointNum - 1;
            float powpracCount = Mathf.Pow(10, pracCount);
            BigInteger intvalue = (BigInteger)(value * powpracCount);
            BigInteger powResult = BigInteger.Pow(intvalue, pow);
            BigInteger Result = BigInteger.Divide(powResult, BigInteger.Pow((BigInteger)powpracCount, pow));
            return Result;
        }
        else//������ ���
        {
            
            BigInteger result = BigInteger.Pow((BigInteger)value, pow);
            return result;
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
        //�⺻������ 2��
        float multipleValue = 2f;
        //������ ������
        if (GameStatus.inst.GetAryRelicLv(3) != 0)
        {
            multipleValue += GameStatus.inst.GetAryRelicLv(3) * GameStatus.inst.RelicDefaultvalue(3);
        }

        // �Ҽ��� ù��° �ڸ� ���� �� ����
        multipleValue = Mathf.Floor(multipleValue * 10) / 10f;

        return DigitAndFloatPercentMultiply(playerDMG, multipleValue);
    }


    /// string * int => return string
    /// <summary>
    /// </summary>
    /// <param name="a"> string / int </param>
    /// <returns></returns>
    public string StringAndIntMultiPly(string value, int multiplyValue)
    {
        sb.Clear();  // StringBuilder �ʱ�ȭ
        BigInteger bigIntValue = BigInteger.Parse(value);
        BigInteger result = BigInteger.Multiply(bigIntValue, multiplyValue);
        return result.ToString();
        //return sb.Append(BigInteger.Multiply(BigInteger.Parse(value), multiplyValue)).ToString();

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


    // ȯ�� ����Ʈ��� �б����� ������

    readonly BigInteger a = new BigInteger(115105889);  // 0.00115105889 * 100000000
    readonly BigInteger b = new BigInteger(50404005900);  // 0.504040059 * 100000000
    readonly BigInteger c = new BigInteger(-12776764200);  // -127.767642 * 100000000
    readonly BigInteger d = new BigInteger(2229519577);  // 22295.19577017339 * 100
    readonly BigInteger scale = new BigInteger(10000000000);  // �����ϸ� ����


    // ��� A�� B�� ����
    BigInteger A = new BigInteger(65);  // �ʱⰪ
    float B = 1.07f;  // ������, �� ���� ���� �����մϴ�.

    /// <summary>
    /// ���� �������� ��������Ͽ� ȯ���� ���޵Ǵ� ����Ʈ���� ����Ͽ� ��Ʈ������ ����
    /// </summary>
    /// <returns> ���ĺ���ȣó�� ���� string ������ Ÿ�� Data </returns>
    public string CurHwansengPoint()
    {
        if (GameStatus.inst == null)
        {
            return null;
        }

        int curStage = GameStatus.inst.AccumlateFloor;

        // B�� BigInteger�� ��ȯ�Ͽ� ���
        BigInteger bigB = new BigInteger(B * 1000); // �����ϸ�
        BigInteger multiplier = BigInteger.One;
        BigInteger baseValue = bigB;
        int exponent = curStage;

        // ���� �������� ����Ͽ� ���
        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
            {
                multiplier *= baseValue;
            }
            baseValue *= baseValue;
            exponent >>= 1;
        }

        // �����ϸ� ����
        BigInteger divisor = BigInteger.Pow(1000, curStage);
        BigInteger reward = (A * multiplier) / divisor;

        return reward.ToString();

    }


    int ulongMaxCount = ulong.MaxValue.ToString().Count();

    /// <summary>
    /// ��ɼ��� ���ݷ°�� : ���� ����ü���� 3% + ������1% �߰�
    /// </summary>
    /// <param name="curEnemyHP"> ���� ����ü�� stirng Data </param>
    /// <returns></returns>
    public string CrewNumber2AtkCalculator(string curEnemyHP)
    {
        int wordCount = curEnemyHP.Count();

        BigInteger hp = new BigInteger();

        int multiPlyer = 3 + GameStatus.inst.Pet2_Lv - 1;

        if (wordCount < ulongMaxCount)
        {
            ulong hpUlong = ulong.Parse(curEnemyHP);
            double CalcurHP = (hpUlong * (ulong)multiPlyer) / 100;


            if (CalcurHP <= 0)
            {
                return null;
            }

            return ((ulong)CalcurHP).ToString();
        }
        else
        {
            hp = BigInteger.Parse(curEnemyHP);
            hp = BigInteger.Divide((BigInteger.Multiply(hp, multiPlyer)), 100);


            if (hp <= 0)
            {
                return null;
            }

            return hp.ToString();
        }


    }
}
