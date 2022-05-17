using System;

namespace ExceptionHandling
{
    class Program
    {

        public static void MetodoQueCausaException()
        {

            /*
             * Vamos supor um método que pode causar uma exception, por exemplo envio de e-mail,
             * acesso a banco de dados ou transferência de dados pela rede.
             * Vamos supor que algo deu errado ma LINHA 20 do arquvivo Program.cs
             */




            throw new Exception("Algo deu muito errado, ABENDOU...");


            //código aqui não acontecerá
        }


        public static void Jeito_Errado_1()
        {

            /*
             * Não é preciso dizer porque isso é errado: está omitindo que houve uma exception, 
             * podendo gerar um erro mais grave ou deixar um processo de negócio em um estado inválido.
             * Nunca faça o "catch mudinho" ou "catch pelado", isso causa bugs difíceis de rastrear, debugar e corrigir
             */

            try
            {
                MetodoQueCausaException();
            }
            catch
            {

            }

            //código aqui vai acontecer, mas o programa poderá estar em um estado inconsistente
        }


        public static void Jeito_Errado_2()
        {
            /*
             * O segundo jeito errado é você fazer um try ... catch para tratar/logar a exception e querer relançá-la 
             * para que ela suba na stack e seja novamente capturada no primeiro chamador ou exibida ao usuário.
             * Muita gente erra aqui em 2 pontos: ou não fazem nada de útil no catch ou fazem um "throw e".
             * "throw e" é prejudicial porque esconde / substitui a stacktrace original, fazendo-o pensar que o 
             * erro está no método e na linha onde você deu "throw e", quando na verdade o erro aconteceu em outro lugar.
             * Se quer interceptar a exceção e colocar uma mensagem mais amigável para o usuário pode usar para isso uma exceção customizada, 
             * mas não esqueça de passar a exceção original na inner exception, para manter registro do lugar original onde ela ocorreu
             */


            try
            {
                MetodoQueCausaException();
            }
            catch(Exception e)
            {

                LogarException(e);

                //ISSO está muito errado, use apenas throw
                throw e; 
            }

            //código aqui não acontecerá
        }


        public static void Jeito_Certo_1()
        {
            try
            {
                MetodoQueCausaException();
            }
            catch(Exception e)
            {
                LogarException(e);
                throw; //e opcionalmente se você quer, depois de logar, interromper a execução do programa e levar a exception acima na stack você simplesmente relança com throw (sem "e")
            }

            //código aqui não acontecerá
        }

        public static void Jeito_Certo_2()
        {
            //se não for logar nem tratar, não faça nada, deixe a exception subir na stack
            MetodoQueCausaException();
        }

        public static void LogarException(Exception e)
        {
            /*
             * IMAGINE AQUI UMA FUNÇÃO QUE TRATA, LOGA, DESTRINCHA UMA EXCEPTION
             * 
             * existem muitas maneiras de capturar o local exato que ocorreu um erro para logarmos: mensagem, arquivo, método e linha. Imagine que essa função faz isso
             */

            Console.WriteLine(e.ToString()); //tratamento ou log da exception. Aqui apenas exibimos seu conteúdo, mas ela pode ser logada com Elmah, App Insights ou coisa parecida. 
        }

        static void Main(string[] args)
        {
            try
            {
                //Jeito_Errado_1(); //repare que a exception não é nem logada nem exibida, ela é engolida e todo código que você esperava que rodasse depois da exception pode não ter rodado. 

                //Jeito_Errado_2(); //repare que embora saibamos que o erro ocorreu no método MetodoQueCausaException linha 20 o Jeito_Errado_2 nos diz que aconteceu em Jeito_Errado_2 linha 72 e ESSE É O PROBLEMA

                //Jeito_Certo_1(); //loga a exception, mas deixa ela ser propagada

                Jeito_Certo_2(); //simplesmente deixa ela ser propagada

                //teste
            }
            catch(Exception err)
            {
                Console.WriteLine(err.ToString());
            }
                 
            Console.ReadLine();
        }
    }
}
