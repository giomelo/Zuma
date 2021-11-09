using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;



namespace Zuma
{
    public enum Cor : int
    {
        Green = (int)ConsoleColor.Green,
        Red = (int)ConsoleColor.Red,
        Blue = (int)ConsoleColor.Blue,
        Yellow = (int)ConsoleColor.Yellow
    }
    public class Peca
    {
        public Cor cor { get; private set; }
        public int num { get; private set; }
        public int posicao { get; set; }
        public Peca(Cor cor, int num, List<Peca> pecas)
        {
            this.cor = cor;
            this.num = num;
            AtualizarPosicao(pecas);
        }
        public void AtualizarPosicao(List<Peca> pecas)
        {
            for (int i = 0; i < pecas.Count; i++)
            {
                if (pecas[i] == this)
                {
                    posicao = i;
                }
            }
        }
    }
    public class Zuma
    {
        public List<Peca> pecas = new List<Peca>();
        Cor[] cores = new Cor[4] { Cor.Green, Cor.Red, Cor.Blue, Cor.Yellow };
        int pontos = 0;
        public int tamanhoMaximo { get; set; } = 10;
        public int tamanhoMinimo { get; set; } = 4;
        public void IniciarJogo()
        {
            Random gerador = new Random();
            for (int i = 0; i < 8; i++)
            {
                int indice = gerador.Next(0, 4);
                Cor corEscolhida = cores[indice];
                Peca peca = new Peca(corEscolhida, 0, pecas);
                pecas.Add(peca);
            }
            foreach (Peca obj in pecas)
            {
                ConsoleColor atualCor = Console.ForegroundColor;
                ConsoleColor corDaPeca = (ConsoleColor)obj.cor;
                Console.ForegroundColor = corDaPeca;
                Console.Write(obj.num);
                Console.ForegroundColor = atualCor;
                Console.Write("|");
            }
            Console.Write("              Pontos:" + pontos);
            Console.WriteLine();
            for (int i = 0; i < pecas.Count; i++)
            {
                Console.Write(i + "|");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        public void ChecarPontos(int index)
        {
            List<Peca> pecasIguaisProx = new List<Peca>();
            int quantidade = 0;
            var pecasMesmaCor = pecas.Where(peca => peca.cor == pecas[index].cor).ToList();
            int posicaoPeca = 0;
            bool direita = false, esquerda = false;
            for (int i = 0; i < pecasMesmaCor.Count; i++)
            {
                if (pecas[index] == pecasMesmaCor[i])
                {
                    posicaoPeca = i;
                }
            }
            if (posicaoPeca == 0)
            {
                quantidade++;
                direita = true;
            }
            else if (posicaoPeca == pecasMesmaCor.Count - 1)
            {
                quantidade++;
                esquerda = true;
            }
            else
            {
                if (Math.Abs(pecasMesmaCor[posicaoPeca].posicao - pecasMesmaCor[posicaoPeca - 1].posicao) == 1)
                {
                    quantidade++;
                    esquerda = true;
                }
                if (Math.Abs(pecasMesmaCor[posicaoPeca].posicao - pecasMesmaCor[posicaoPeca + 1].posicao) == 1)
                {
                    quantidade++;
                    direita = true;
                }
            }
            if (quantidade == 1)
            {
                if (direita)
                {
                    quantidade = 0;
                    for (int i = posicaoPeca; i < pecasMesmaCor.Count - 1; i++)
                    {
                        if (Math.Abs(pecasMesmaCor[posicaoPeca].posicao - pecasMesmaCor[posicaoPeca + 1].posicao) == 1)
                        {
                            quantidade++;
                            pecasIguaisProx.Add(pecasMesmaCor[posicaoPeca + 1]);
                            posicaoPeca = posicaoPeca + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (esquerda)
                {
                    quantidade = 0;
                    for (int i = posicaoPeca; i > 0; i--)
                    {
                        if (Math.Abs(pecasMesmaCor[posicaoPeca].posicao - pecasMesmaCor[posicaoPeca - 1].posicao) == 1)
                        {
                            quantidade++;
                            pecasIguaisProx.Add(pecasMesmaCor[posicaoPeca - 1]);
                            posicaoPeca = posicaoPeca - 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else if (quantidade == 2)
            {
                quantidade = 0;
                for (int i = posicaoPeca; i < pecasMesmaCor.Count - 1; i++)
                {
                    if (Math.Abs(pecasMesmaCor[posicaoPeca].posicao - pecasMesmaCor[posicaoPeca + 1].posicao) == 1)
                    {
                        quantidade++;
                        pecasIguaisProx.Add(pecasMesmaCor[posicaoPeca + 1]);
                        posicaoPeca = posicaoPeca + 1;
                    }
                    else
                    {
                        break;
                    }
                }
                for (int i = 0; i < pecasMesmaCor.Count; i++)
                {
                    if (pecas[index] == pecasMesmaCor[i])
                    {
                        posicaoPeca = i;
                    }
                }
                for (int i = posicaoPeca; i > 0; i--)
                {
                    if (Math.Abs(pecasMesmaCor[posicaoPeca].posicao - pecasMesmaCor[posicaoPeca - 1].posicao) == 1)
                    {
                        quantidade++;
                        pecasIguaisProx.Add(pecasMesmaCor[posicaoPeca - 1]);
                        posicaoPeca = posicaoPeca - 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (quantidade >= 2)
            {
                pecasIguaisProx.Add(pecas[index]);
                RemoverPeca(pecasIguaisProx);
                pontos += quantidade + 1;
                foreach (Peca pecasInGame in pecas)
                {
                    pecasInGame.AtualizarPosicao(pecas);
                }
                ChecarPontos(index - quantidade);
            }
        }
        public bool ChecarVitoria()
        {
            if (pecas.Count <= tamanhoMinimo)
            {
                return true;
            }
            return false;
        }
        public bool ChecarDerrota()
        {
            if (pecas.Count >= tamanhoMaximo)
            {
                return true;
            }
            return false;
        }
        public void ImprimirTabuleiro()
        {
            Console.Clear();
            foreach (Peca obj in pecas)
            {
                ConsoleColor atualCor = Console.ForegroundColor;
                ConsoleColor corDaPeca = (ConsoleColor)obj.cor;
                Console.ForegroundColor = corDaPeca;
                Console.Write(obj.num);
                Console.ForegroundColor = atualCor;
                Console.Write("|");
            }
            Console.Write("              Pontos:" + pontos);
            Console.WriteLine();
            for (int i = 0; i < pecas.Count; i++)
            {
                Console.Write(i + "|");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        public void ImprimirPeca(Peca peca)
        {
            ConsoleColor atualCor = Console.ForegroundColor;
            ConsoleColor corDaPeca = (ConsoleColor)peca.cor;
            Console.ForegroundColor = corDaPeca;
            Console.Write(peca.num);
            Console.ForegroundColor = atualCor;
        }
        public bool InserirPeca(int index, Peca peca)
        {
            if (index < pecas.Count && index > 0)
            {
                pecas.Insert(index, peca);
                /* if (pecas[index] == pecas[pecas.Count - 2]) 
                 { 
                     Peca aux = pecas[pecas.Count - 1]; 
                     pecas[pecas.Count - 1] = pecas[index]; 
                     pecas[index] = aux; 
                     return false; 
                 }*/
                return false;
            }
            return true;
        }
        public void RemoverPeca(List<Peca> pecasParaRemover)
        {
            foreach (Peca item in pecasParaRemover)
            {
                pecas.Remove(item);
            }
        }
        public Peca PecaAleatoria()
        {
            Random gerador = new Random();
            int indice = gerador.Next(0, 4);
            Cor corEscolhida = cores[indice];
            Peca peca = new Peca(corEscolhida, 0, pecas);
            return peca;
        }
        public void ReiniciarJogo()
        {
            Console.Clear();
            pontos = 0;
            pecas.Clear();
            IniciarJogo();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Zuma jogo = new Zuma();
            jogo.IniciarJogo();
            Console.WriteLine("Bem vindo ao jogo Zuma! Seu objetivo é deixar a fileira de cores com " + jogo.tamanhoMinimo + " itens, se ela chegar a " + jogo.tamanhoMaximo + " você perde." +
                "Boa Sorte!");
            Console.WriteLine("REGRAS: Quando inserida uma cor ao lado de no mínimo 2 iguais, elas são destruidas e é contado pontos.");
            Console.ReadKey();
            Jogar(jogo);
        }
        static void Jogar(Zuma jogo)
        {
            Peca pecaEscolhida = null;
            while (!jogo.ChecarDerrota())
            {
                try
                {
                    int posicao = jogo.pecas.Count - 1;
                    pecaEscolhida = jogo.PecaAleatoria();
                    Console.WriteLine("Sua peça atual é: ");
                    jogo.ImprimirPeca(pecaEscolhida);
                    Console.WriteLine();
                    try
                    {
                        do
                        {
                            Console.WriteLine("Em qual posição deseja inserir a peça? Você tem 20 segundos para fazer essa decisão!");
                            posicao = int.Parse(Reader.ReadLine(20000));
                        } while (jogo.InserirPeca(posicao, pecaEscolhida));
                        foreach (Peca pecasInGame in jogo.pecas)
                        {
                            pecasInGame.AtualizarPosicao(jogo.pecas);
                        }
                        jogo.ChecarPontos(posicao);
                        jogo.ImprimirTabuleiro();
                        if (jogo.ChecarVitoria())
                        {
                            Console.WriteLine("Você ganhou!");
                            Console.WriteLine("O jogo será reiniciado em 5 segundos.");
                            Thread.Sleep(5000);
                            jogo.ReiniciarJogo();
                            Jogar(jogo);
                        }
                    }
                    catch (TimeoutException)
                    {
                        posicao = jogo.pecas.Count - 1;
                        jogo.InserirPeca(posicao, pecaEscolhida);
                        foreach (Peca pecasInGame in jogo.pecas)
                        {
                            pecasInGame.AtualizarPosicao(jogo.pecas);
                        }
                        jogo.ChecarPontos(posicao);
                        jogo.ImprimirTabuleiro();
                        if (jogo.ChecarVitoria())
                        {
                            Console.WriteLine("Você ganhou!");
                            Console.WriteLine("O jogo será reiniciado em 5 segundos.");
                            Thread.Sleep(5000);
                            jogo.ReiniciarJogo();
                            Jogar(jogo);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (jogo.ChecarDerrota())
            {
                Console.WriteLine("Você perdeu!!");
                Console.WriteLine("O jogo será reiniciado em 5 segundos.");
                Thread.Sleep(5000);
                jogo.ReiniciarJogo();
                Jogar(jogo);
            }
        }
    }
    class Reader
    {
        private static Thread inputThread;
        private static AutoResetEvent getInput, gotInput;
        private static string input;
        static Reader()
        {
            getInput = new AutoResetEvent(false);
            gotInput = new AutoResetEvent(false);
            inputThread = new Thread(reader);
            inputThread.IsBackground = true;
            inputThread.Start();
        }
        private static void reader()
        {
            while (true)
            {
                getInput.WaitOne();
                input = Console.ReadLine();
                gotInput.Set();
            }
        }
        public static string ReadLine(int timeOutMillisecs = Timeout.Infinite)
        {
            getInput.Set();
            bool success = gotInput.WaitOne(timeOutMillisecs);
            if (success)
                return input;
            else
                throw new TimeoutException("User did not provide input within the timelimit.");
        }
    }
}