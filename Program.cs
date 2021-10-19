using System;
using System.Collections.Generic;

namespace Observer_State_Strategy
{
    #region Observer

    interface IObserver
    {
        void Update(Object ob);
    }

    interface IObservable
    {
        void RegisterObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObservers();
    }

    class Stock : IObservable
    {
        StockInfo sInfo; // информация о торгах

        List<IObserver> observers;
        public Stock()
        {
            observers = new List<IObserver>();
            sInfo = new StockInfo();
        }
        public void RegisterObserver(IObserver o)
        {
            observers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            observers.Remove(o);
        }

        public void NotifyObservers()
        {
            foreach (IObserver o in observers)
            {
                o.Update(sInfo);
            }
        }

        public void Market()
        {
            Random rnd = new Random();
            sInfo.USD = rnd.Next(20, 40);
            sInfo.Euro = rnd.Next(30, 50);
            NotifyObservers();
        }
    }

    class StockInfo
    {
        public int USD { get; set; }
        public int Euro { get; set; }
    }

    class Broker : IObserver
    {
        public string Name { get; set; }
        IObservable stock;
        public Broker(string name, IObservable obs)
        {
            this.Name = name;
            stock = obs;
            stock.RegisterObserver(this);
        }
        public void Update(object ob)
        {
            StockInfo sInfo = (StockInfo)ob;

            if (sInfo.USD > 30)
                Console.WriteLine("Брокер {0} продает доллары;  Курс доллара: {1}", this.Name, sInfo.USD);
            else
                Console.WriteLine("Брокер {0} покупает доллары;  Курс доллара: {1}", this.Name, sInfo.USD);
        }
        public void StopTrade()
        {
            stock.RemoveObserver(this);
            stock = null;
        }
    }

    class Bank : IObserver
    {
        public string Name { get; set; }
        IObservable stock;
        public Bank(string name, IObservable obs)
        {
            this.Name = name;
            stock = obs;
            stock.RegisterObserver(this);
        }
        public void Update(object ob)
        {
            StockInfo sInfo = (StockInfo)ob;

            if (sInfo.Euro > 40)
                Console.WriteLine("Банк {0} продает евро;  Курс евро: {1}", this.Name, sInfo.Euro);
            else
                Console.WriteLine("Банк {0} покупает евро;  Курс евро: {1}", this.Name, sInfo.Euro);
        }
    }

    #endregion Observer

    #region State

    class Water
    {
        public IWaterState State { get; set; }

        public Water(IWaterState ws)
        {
            State = ws;
        }

        public void Heat()
        {
            State.Heat(this);
        }
        public void Frost()
        {
            State.Frost(this);
        }
    }

    interface IWaterState
    {
        void Heat(Water water);
        void Frost(Water water);
    }

    class SolidWaterState : IWaterState
    {
        public void Heat(Water water)
        {
            Console.WriteLine("Превращаем лед в жидкость");
            water.State = new LiquidWaterState();
        }

        public void Frost(Water water)
        {
            Console.WriteLine("Продолжаем заморозку льда");
        }
    }
    class LiquidWaterState : IWaterState
    {
        public void Heat(Water water)
        {
            Console.WriteLine("Превращаем жидкость в пар");
            water.State = new GasWaterState();
        }

        public void Frost(Water water)
        {
            Console.WriteLine("Превращаем жидкость в лед");
            water.State = new SolidWaterState();
        }
    }
    class GasWaterState : IWaterState
    {
        public void Heat(Water water)
        {
            Console.WriteLine("Повышаем температуру водяного пара");
        }

        public void Frost(Water water)
        {
            Console.WriteLine("Превращаем водяной пар в жидкость");
            water.State = new LiquidWaterState();
        }
    }

    #endregion State

    #region Strategy

    interface IMovable
    {
        void Move();
    }

    class PetrolMove : IMovable
    {
        public void Move()
        {
            Console.WriteLine("Перемещение на бензине");
        }
    }

    class ElectricMove : IMovable
    {
        public void Move()
        {
            Console.WriteLine("Перемещение на электричестве");
        }
    }
    class Car
    {
        protected int passengers;
        protected string model;

        public Car(int num, string model, IMovable mov)
        {
            this.passengers = num;
            this.model = model;
            Movable = mov;
        }
        public IMovable Movable { private get; set; }
        public void Move()
        {
            Movable.Move();
        }
    }

    #endregion Strategy

    class Program
    {
        static void Main(string[] args)
        {
            #region Observer

            //Stock stock = new Stock();
            //Bank bank = new Bank("ЮнитБанк", stock);
            //Broker broker = new Broker("Иван Иваныч", stock);
            //// имитация торгов
            //stock.Market();
            //// брокер прекращает наблюдать за торгами
            //broker.StopTrade();
            //// имитация торгов
            //stock.Market();

            //Console.Read();

            #endregion Observer

            #region State

            //Water water = new Water(new LiquidWaterState());
            //water.Heat();
            //water.Frost();
            //water.Frost();

            //Console.Read();

            //#endregion State

            //#region Strategy



            #endregion Strategy

            #region Strategy

            Car auto = new Car(4, "Volvo", new PetrolMove());
            auto.Move();
            auto.Movable = new ElectricMove();
            auto.Move();

            Console.ReadLine();

            #endregion Strategy

        }
    }
}
