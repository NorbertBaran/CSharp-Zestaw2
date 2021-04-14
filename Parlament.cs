using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Cw2{

    class Parliament{
        protected Parliamenterian[] Parliamenterians;
        protected string Topic;
        protected int TrueVoteCount;
        protected int FalseVoteCount;
        public delegate void VoteBeginHandler(string topic);
        public event VoteBeginHandler VoteBegin;
        public delegate void VoteEndHandler();
        public event VoteEndHandler VoteEnd;

        public Parliament(int parliamentariansCount, string topic){
            Parliamenterians = new Parliamenterian[parliamentariansCount];
            for(int i=0; i<Parliamenterians.Length; i++)
                Parliamenterians[i] = new Parliamenterian(this);
            Topic = topic;
            foreach(var parliamenterian in Parliamenterians){
                VoteBegin+=parliamenterian.VoteBegin;
                VoteEnd+=parliamenterian.VoteEnd;
            }
            TrueVoteCount = 0;
            FalseVoteCount = 0;
        }

        void GetVoteResult(){
            Console.WriteLine("Głosowanie nad {0} Głosów za {1} Głosów przeciw {2}", Topic, TrueVoteCount, FalseVoteCount);
        }

        static void Main(string[] args){
            Console.WriteLine("Witamy w parlamencie. Wprowadź liczbę parlamentarzystów oraz temat głosowania oddzielone spacją.");
            int parlamentariansCount=0;
            string topic="";
            string command;

            do{
                command = Console.ReadLine();
                if(Regex.IsMatch(command, "^[0-9]+ .*")){
                    parlamentariansCount = int.Parse(command.Substring(0, command.IndexOf(" ")));
                    topic = command.Substring(command.IndexOf(" ")+1);
                    Console.WriteLine("Oczekiwanie na rozpoczęcie głosowania...");
                    break;
                } else
                    Console.WriteLine("Nieprawidowe dane. Spróbuj ponownie.");
                Console.WriteLine(command);
            }while(Regex.IsMatch(command, "^[0-9]+ .*"));

            Parliament parliament = new Parliament(parlamentariansCount, topic);
            
            do{
                command=Console.ReadLine();
                if(command == "POCZATEK " + parliament.Topic)
                    parliament.OnVoteBegin();
                else if(Regex.IsMatch(command, "^GŁOS [0-9]+$")){
                    int parliamenterianNumber = int.Parse(command.Substring(5));
                    if(parliamenterianNumber >= 0 && parliamenterianNumber < parliament.Parliamenterians.Length)
                        parliament.Parliamenterians[parliamenterianNumber].OnVote();
                    else
                        Console.WriteLine("Brak parlamentarzysty o wskazanym numerze. Parlamentarzyści: 0-{0}", parliament.Parliamenterians.Length-1);
                }
                else if(command!="KONIEC")
                    Console.WriteLine("Nie zidentyfikowano polecenia. Spróbuj ponownie. W przypadku głosowania pamiętaj, aby polecenie oddzielić od argumentu spacją.");
                
            }while(command!="KONIEC");
            
            parliament.OnVoteEnd();
            parliament.GetVoteResult();
        }

        protected virtual void OnVoteBegin(){
            VoteBegin?.Invoke(Topic);
            Console.WriteLine("Rozpoczęto głosowanie.");
        }

        protected virtual void OnVoteEnd(){
            VoteEnd?.Invoke();
        }

        public void Vote(bool vote){
            if(vote)
                TrueVoteCount++;
            else
                FalseVoteCount++;
            Console.WriteLine("Twój głos został przyjęty");
        }
    }

    class Parliamenterian{
        public delegate void VoteHandler(bool vote);
        public event VoteHandler Vote;
        protected bool VotingTime=false;
        public Parliamenterian(Parliament parliament){
            Vote += parliament.Vote;
        }

        public virtual void OnVote(){
            if(VotingTime){
                Vote?.Invoke(new Random().NextDouble() >= 0.5);
                VotingTime = false;
            }
            else
                Console.WriteLine("Głosowanie jeszcze nie zostało rozpoczęte lub głos twój został już oddany.");
        }

        public void VoteBegin(string topic){
            VotingTime = true;
        }

        public void VoteEnd(){
            VotingTime = false;
        }
    }
}