using System.Text;

namespace FlexiBase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test app FLEXiBASE");
            Console.OutputEncoding = Encoding.UTF8;

            /*
             * 
             * Напишите программу, которая будет принимать на вход список чисел,
             * а выводить тот же список чисел, но с заменами. 
             * Использовать ООП и SOLID,  написать тесты.
               Если число делится на 3 без остатка - заменить его на "fizz", 
               если делится на 5 без остатка - заменить его на "buzz", 
              если делится на 3 и на 5 без остатка - заменить на "fizz-buzz"
                In: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 */

            Console.WriteLine("Test #1");
            List<Instuction> test_1 = new List<Instuction>();
            test_1.Add(new Instuction(3, "fizz", "add"));
            test_1.Add(new Instuction(5, "buzz", "add"));

            int[] digits = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15  };
            Replacer replacer_1 = new Replacer(digits,test_1);
            Console.WriteLine ($"> {replacer_1.GetMessage} " );

            //Out: 1, 2, fizz, 4, buzz, fizz, 7, 8, fizz, buzz, 11, fizz, 13, 14, fizz-buzz
            if (replacer_1.SelfTest("1, 2, fizz, 4, buzz, fizz, 7, 8, fizz, buzz, 11, fizz, 13, 14, fizz-buzz") )
            {
                Console.WriteLine("TEST PASSED");
            }
            replacer_1.Dispose();
            



            Console.WriteLine("Test #2");
            List<Instuction> test_2 = new List<Instuction>();
            
            test_2.Add(new Instuction(3, "fizz", "add"));
            test_2.Add(new Instuction(5, "buzz", "add"));
            test_2.Add(new Instuction(4, "muzz", "add"));
            test_2.Add(new Instuction(7, "guzz", "add"));



            digits = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 60, 105, 420 };
            Replacer replacer_2 = new Replacer(digits, test_2);
            Console.WriteLine ($"> {replacer_2.GetMessage} ");

            //Out: 1, 2, fizz, muzz, buzz, fizz, guzz, muzz,
            // fizz, buzz, 11, fizz-muzz, 13, guzz, fizz-buzz,
            // fizz-buzz-muzz, fizz-buzz-guzz, fizz-buzz-muzz-guzz

            if (replacer_2.SelfTest("1, 2, fizz, muzz, buzz, fizz, guzz, muzz, fizz, buzz, 11, fizz-muzz, 13, guzz, fizz-buzz, fizz-buzz-muzz, fizz-buzz-guzz, fizz-buzz-muzz-guzz"))
            {
                Console.WriteLine("TEST PASSED");
            }

            replacer_2.Dispose();



            Console.WriteLine("Test #3");
            List<Instuction> test_3 = new List<Instuction>();

            
            test_3.Add(new Instuction(3, "dog", "replace"));
            test_3.Add(new Instuction(5, "cat", "replace"));
            test_3.Add(new Instuction(15, "good-boy", "replace"));  //  3*5 одновременно
            test_3.Add(new Instuction(4, "muzz", "add"));
            test_3.Add(new Instuction(7, "guzz", "add"));
            




            digits = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 60, 105, 420 };
            Replacer replacer_3 = new Replacer(digits, test_3);
            Console.WriteLine($"> {replacer_3.GetMessage} ");

            //Out: 1, 2, dog, muzz, cat, dog, guzz, muzz, dog, cat, 11, fizz-muzz,
            //13, guzz, good-boy, good-boy-muzz, good-boy-guzz, good-boy-muzz-guzz

            //ТУТ ОШИБКА НА fizz - был в 1 и 2 тесте, в 3 тесте его не БУДЕТ fizz->dog!
            if (replacer_3.SelfTest("1, 2, dog, muzz, cat, dog, guzz, muzz, dog, cat, 11, fizz-muzz, 13, guzz, good-boy, good-boy-muzz, good-boy-guzz, good-boy-muzz-guzz"))
            {
                Console.WriteLine("TEST PASSED");
            }

            //ТУТ ОШИБКА НА fizz - был в 1 и 2 тесте, в 3 тесте его не БУДЕТ!
            if (replacer_3.SelfTest("1, 2, dog, muzz, cat, dog, guzz, muzz, dog, cat, 11, dog-muzz, 13, guzz, good-boy, good-boy-muzz, good-boy-guzz, good-boy-muzz-guzz"))
            {
                Console.WriteLine("TEST PASSED");
            }


        }
    }


    /// <summary>
    /// Класс инструкций по замене данных
    /// </summary>
    class Instuction :IDisposable
    {

        /// <summary>
        /// Делители
        /// </summary>
        public int _mod_divver      { set; get; } = 0;


        /// <summary>
        /// Заменители
        /// </summary>
        public string _replacer     { set; get;  } = ""; //buzz wooz etc

        /// <summary>
        /// Инструкция add - добавить к результату
        /// replace  - заменить
        /// </summary>
        public string _instruction = "add"; //add or replace

        public Instuction(int md, string replacer, string instruction) 
        { 
            _mod_divver= md;
            _replacer= replacer;
            _instruction= instruction;
        }

        /// <summary>
        /// Уничтожает струкутры
        /// </summary>
      public void Dispose()
        {
            _instruction = "";
            _mod_divver = 0;
            _replacer = "";
            GC.Collect();
        }


    }

    /// <summary>
    /// Заменитель данных в массиве
    /// </summary>
    class Replacer    :IDisposable
    {

        private List<string> result = null;
        readonly int[] saved = null;

        public string[] GetResult 
        {
            get { return result.ToArray(); }

        }

        public string GetMessage
        {
            get 
            {   string msg = "";

                if (result != null)
                {
                    foreach (string s in result)
                    { msg += " " + s + ","; }

                    //Удалим последнюю ,
                    msg = msg.Substring(0, msg.Length - 1);

                }
                else
                { msg = "NO DATA"; }
                return msg;
            }

        }

        public Replacer(int[] digits, List<Instuction> instuctions)
        {

            saved = digits;
            result = new List<string>();
            for (int i = 0; i < digits.Length; i++) 
            { 
                string new_value = "";
                //обходим интрукции
                foreach (Instuction instr in instuctions)
                {
                    if (digits[i] % instr._mod_divver == 0)
                    { 
                      switch (instr._instruction)
                        {
                            case "add":
                                if (new_value != "")
                                      new_value += "-";

                                new_value += instr._replacer;
                                break;
                            
                            case "replace":
                                new_value = instr._replacer;
                                break;
                        }

                    }
                }

                //если инструкций нет - то просто значение
                if (new_value.Equals(""))
                {
                    new_value = digits[i].ToString(); 
                }

                //Создание результата
                result.Add(new_value);
              
            }
        
        }


        public bool SelfTest(string inp_test)
        {
            string[] x = inp_test.Split(",", StringSplitOptions.None);

            string DEBUG = "";
            int i=0;
            int scores = 0;

            
            foreach (string s in x) 
            {

             string tmp = s.Trim();

             bool its_ok = result[i] == tmp;
             scores =  its_ok  ? scores+1 : scores ;

            DEBUG += its_ok ? " ✔ " + result[i] : "  x " + tmp + " <> " + result[i] + " ( " + saved[i] +")";
            i++;

                
            }


            if (scores != x.Length)
            {
                Console.WriteLine($"Что-то не так: \r\n" + DEBUG);
            }
            else
            {
                Console.WriteLine("Самотест - OK :\r\n" +DEBUG);
            }
            return scores == x.Length;

        }


        public void Dispose()
        {
            if (result!=null)
                result.Clear();


            GC.Collect();
        }

    }     // REPLACER





}