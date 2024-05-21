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
    /// 
    /// </summary>
    /// <param name="a"> A 값</param>
    /// <param name="b"> B 값</param>
    /// <param name="areYouEnemy"> 만약 몬스터라면 true 로 매개변수 입력 / 일반 계산이라면 false</param>
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

        if (areYouEnemy == true && int.Parse(A[0].ToString()) < int.Parse(B[0].ToString())) // 만약 결과가 음수로 될시 죽음처리
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

        return result; // 결과가 빈 문자열인 경우 0이니깐 "Dead"을 반환
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

        
        if(curHP <= 0) 
        {
            result = "0";
        }
        else if(curHP > 0) 
        {
            result = curHP.ToString();
        }

        return result;
    }

    /// <summary>
    /// %계산기 : StringFourDigitChanger 함수 들어갔다 나온 문자열 + % 집어넣어야함
    /// </summary>
    /// <param name="a"> Ex : 555A </param>
    /// <param name="percent"> 5% => 5 </param>
    /// <returns></returns>
    public string DigitPercentMultiply(string a, int percent)
    {
        // 입력 문자열을 BigInteger로 파싱
        BigInteger original = BigInteger.Parse(a);

        // percent를 사용하여 증가할 비율을 BigInteger로 계산
        // 예: percent가 5라면, totalPercent는 105% 즉, 1.05가 되어야함
        // BigInteger는 소수점을 지원하지 않으므로, 분수로 처리
        BigInteger numerator = 100 + percent;
        BigInteger denominator = 100;

        // 결과 계산: (original * numerator) / denominator
        // 분자에 original과 numerator를 곱하고, 분모로 denominator를 사용
        BigInteger result = (original * numerator) / denominator;

        return result.ToString();
    }



    /// <summary>
    /// 모든공격력 합산하여 스트링으로 변환하여 리턴
    /// </summary>
    /// <returns></returns>
    public string Get_CurPlayerATK()
    {
        string result = GameStatus.inst.TotalAtk.ToString();

        string[] addBuffValue = new string[4];
        
        // 2배 버프 체크
        // { //result = DigidPlus(result, GameStatus.inst.BuffAddATK);}

        if (GameStatus.inst.BuffAddATK != "0")
        {
            addBuffValue[0] = StringAndIntMultiPly(result, 2-1);
        }

        //// 3배 버프 체크
        //result = DigidPlus(result, GameStatus.inst.BuffAddAdATK);
        if (GameStatus.inst.BuffAddATK != "0")
        {
            addBuffValue[1] = StringAndIntMultiPly(result, 3-1);
        }

        ////// 초심자 버프 2배
        //result = DigidPlus(result, GameStatus.inst.NewbieATKBuffValue);
        if (GameStatus.inst.NewbieATKBuffValue != "0")
        {
            addBuffValue[2] = StringAndIntMultiPly(result, 2-1);
        }

        //// 펫 버프량 체크 { 레벨당 10% 증가}
        //result = DigidPlus(result, GameStatus.inst.AddPetAtkBuff);
        if (GameStatus.inst.AddPetAtkBuff != "0")
        {
            addBuffValue[3] = DigitPercentMultiply(result, GameStatus.inst.Pet0_Lv * 10);
        }

        ///// 무기도감 카운트만큼 % 합산
        //result = DigitPercentMultiply(result, DogamManager.inst.weaponDogamGetCount);


        for (int index = 0; index < addBuffValue.Length; index++)
        {
            
            if (addBuffValue[index] != null)
            {
                result = DigidPlus(result, addBuffValue[index]);
            }
        }

        return result;
    }

    public string ToBigIntigerFromString(BigInteger value)
    {
        return value.ToString();
    }

    // 선형구조
    //public BigInteger baseHP = new BigInteger(100);  // 기본 체력
    //public BigInteger hpIncreaseFactor = new BigInteger(15);  // 층당 체력 증가 계수 (밸런스 변수)

    //public string EnemyHpSetup()
    //{
    //    int floor = GameStatus.inst.AccumlateFloor+1;
    //    BigInteger health = baseHP + hpIncreaseFactor * floor * floor; // 층수의 제곱을 이용한 체력 계산

    //    //Debug.Log($"누적층수: {floor}, 초기화된 체력: {health}");
    //    return health.ToString();
    //}
    public BigInteger baseHP = new BigInteger(160);  // 기본 체력
    public double hpIncreaseFactor = 1.05;  // 체력 증가 계수 (밸런스 변수)

    public string EnemyHpSetup()
    {
        // 현재 층수를 가져옴 (0부터 시작한다고 가정)
        int floor = GameStatus.inst.AccumlateFloor + 1;

        // 체력 계산: 기본 체력 + (체력 증가 계수 * 층수의 세제곱)
        BigInteger health = baseHP + new BigInteger(hpIncreaseFactor * Mathf.Pow(floor, 3));

        // 체력 로그 출력 (디버그 용도)
        // Debug.Log($"누적층수: {floor}, 초기화된 체력: {health}");

        // 체력을 문자열로 반환
        return health.ToString();
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
            //word += (char)(64 + index); 아스키코드 과정에서 변경 A -> ZZ
            word = MakeBigDigitWord(index);
        }

        return digit + word;
    }



    /// <summary>
    /// 소수점 포함 숫자문자 변환기 ( Ex : 1.1A , 232.1B 등 )
    /// </summary>
    /// <param name="inputValue"></param>
    /// <returns></returns>
    public string StringFourDigitAddFloatChanger(string inputValue)
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

        if (inputValue.Length > 3) //소수점 붙여줌
        {
            digit += ".";
            digit += new string(inputValue.Skip(digitLength).Take(1).ToArray()); // 소수점 한자리숫자 가져옴
        }

        // 알파벳 가져오는 부분
        if (index > 0)
        {
            //word += (char)(64 + index); 아스키코드 과정에서 변경 A -> ZZ
            word = MakeBigDigitWord(index);
        }

        return digit + word;
    }



    /// <summary>
    /// 숫자뒤에 문자로 합산해주는 함수
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private string MakeBigDigitWord(int index)
    {
        string result = string.Empty;
        while (index > 0)
        {
            index--; // 0 기반으로 조정
            int remainder = index % 26;
            char letter = (char)('A' + remainder);
            result = letter + result;
            index = (index / 26); // 정확한 나눗셈
        }
        return result;
    }
    /// <summary>
    /// 숫자뒤의 문자를 숫자로 합산시켜주는 함수
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
            _text = _text.Remove(pointindex,1);
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
    /// 문자 제외 앞자리 숫자만 리턴
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


    /// string * int => return string
    /// <summary>
    /// </summary>
    /// <param name="a"> string / int </param>
    /// <returns></returns>
    public string StringAndIntMultiPly(string value, int multiplyValue)
    {
        sb.Clear();  // StringBuilder 초기화
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

        // 분자를 확장하여 소수점 이하 정밀도 확보
        BigInteger scale = BigInteger.Pow(10, precision);
        BigInteger expandedA = bigA * scale;

        // 확장된 분자를 분모로 나눔
        BigInteger result = expandedA / bigB;

        // 결과를 문자열로 변환
        string resultString = result.ToString();

        // 결과 문자열에 소수점 추가
        if (resultString.Length <= precision)
        {
            // 결과가 1보다 작은 경우, 선행 0을 추가
            resultString = resultString.PadLeft(precision + 1, '0');
        }

        // 적절한 위치에 소수점 삽입
        string finalResult = resultString.Insert(resultString.Length - precision, ".");

        // 소수점 이하 불필요한 0 제거 (옵션)
        finalResult = finalResult.TrimEnd('0').TrimEnd('.');
        return float.Parse(finalResult);
    }


    // 환생 포인트계산 읽기전용 변수들

    readonly BigInteger a = new BigInteger(115105889);  // 0.00115105889 * 100000000
    readonly BigInteger b = new BigInteger(50404005900);  // 0.504040059 * 100000000
    readonly BigInteger c = new BigInteger(-12776764200);  // -127.767642 * 100000000
    readonly BigInteger d = new BigInteger(2229519577);  // 22295.19577017339 * 100
    readonly BigInteger scale = new BigInteger(10000000000);  // 스케일링 팩터


    BigInteger A = new BigInteger(65);  // 초기값
    float B = 1.26f;  // 증가율, 이 값은 조정 가능합니다.

    /// <summary>
    /// 현재 스테이지 레벨비례하여 환생시 지급되는 포인트양을 계산하여 스트링으로 받음
    /// </summary>
    /// <returns> 알파벳부호처리 안한 string 정수형 타입 Data </returns>
    public string CurHwansengPoint()
    {
        if(GameStatus.inst == null)
        {
            return null;
        }
        string result = string.Empty;

        int curStage = GameStatus.inst.AccumlateFloor;


        // 상수 A와 B를 정의합니다.


        // 지급되는 '별' 화폐의 양을 계산합니다.
        BigInteger reward = A * new BigInteger(Mathf.Pow(B, curStage));

        // 결과를 문자열로 반환합니다.
        return reward.ToString();
        // 현재 스테이지가 올라감에 있어 초반에는 조금씩 후반에는 급격하게 지급되는양이 증가되게

    }


    int ulongMaxCount = ulong.MaxValue.ToString().Count();
    
    /// <summary>
    /// 사령술사 공격력계산 : 현재 몬스터체력의 3% + 레벨당1% 추가
    /// </summary>
    /// <param name="curEnemyHP"> 현재 몬스터체력 stirng Data </param>
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
