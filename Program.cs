using System;

namespace Cw2
{
    interface IFoo{
        event EventHandler Boom1;
    }

    class Program{
        static void main(string[] args){
            Action a = () => Console.WriteLine("!");
            a += () => Console.WriteLine("?");


            var boomer = new Boomer();
            boomer.Boom += boomer_Boom;
            boomer.Sart();
            a();

            // boomer.Boom(null, null);
            // boomer.Boom=null;
        }
        
        private static void boomer_Boom(object sender, EventArgs e){
            Console.WriteLine("Boom");
        }

    }

    class Boomer{
        public event EventHandler Boom;

        public void Sart(){
            OnBoom();
            OnBoom();
            OnBoom();
        }

        protected  virtual void OnBoom(){
            Boom?.Invoke(this, EventArgs.Empty);
        }
    }

    public delegate void EventHandler(object sender, EventArgs e);

}
