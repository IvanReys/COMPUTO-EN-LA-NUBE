////////////////////////////////////////////////////////////////
// IVAN REYES 33908 - COMPUTO EN LA NUBE - 17/08/2026         //
// Actividad: Tarea - Cena de Filosofos                       //
// Instrucciones de la Actividad:                             //
// - Programa en C# que represente la Cena de los Filósofos   //
// - Publicar repositorio publico de github                   //
// - Publicar un archivo cs con al menos 3 clases:            //
// - Application/MainApplicaiton                              //
// - Fork                                                     //
// - Philosopher                                              //
////////////////////////////////////////////////////////////////

using System;
using System.Threading;

namespace CenaFilosofos
{
    // CLASE 1 - FORK
    // para el recurso compartido que es el tenedco
    public class Fork
    {
        public int Id { get; private set; }
        public Fork(int id) { // constructor que recibe el id del tenedor
            Id = id;
        }
    }

    // CLASE 2 - PHILOSOPHER
    // para los hilos que son los filosofos
    public class Philosopher
    {
        public int Id { get; private set; }
        private Fork FirstFork { get; set; }  // primer tenedor que va a tomar
        private Fork SecondFork { get; set; } // segundo tenedor que va a tomar
        private Thread PhilosopherThread;     // hilo del filosofo

        public Philosopher(int id, Fork left, Fork right){ // constructor que recibe el id del filosofo y los tenedores
            Id = id;

            // para evitar el deadlock 
            // cada filosofo tomara primero el tenedor con el id mas bajo y luego el tenedor con el id mas alto
            if (left.Id < right.Id) {
                FirstFork = left;
                SecondFork = right;
            }
            else {
                FirstFork = right;
                SecondFork = left;
            }

            PhilosopherThread = new Thread(Dine);
        }
        
        public void Start() => PhilosopherThread.Start(); 
        public void Join() => PhilosopherThread.Join();

        private void Dine() { // para que el filosofo piense y coma
            for (int i = 0; i < 1; i++) {
                Think();
                Eat();
            }
        }

        private void Think() { // simula que el filosofo esta pensadno
            Console.WriteLine($"Filosofo {Id} PENSANDO...");
            Thread.Sleep(new Random().Next(500, 1500));
        }

        private void Eat() {
            Console.WriteLine($"Filoso {Id} INTENTANDO TOMAR el tenedor {FirstFork.Id}...");
            lock (FirstFork) {
                Console.WriteLine($"Filoso {Id} TOMO el tenedor {FirstFork.Id} y esta ESPERANDO el otro tenedor {SecondFork.Id}...");
                lock (SecondFork) {
                    Console.WriteLine($"Filoso {Id} esta COMIENDO con el tenedor {FirstFork.Id} y {SecondFork.Id}.");
                    Thread.Sleep(new Random().Next(500, 1500)); 
                    Console.WriteLine($"Filoso {Id} TERMINO de comer y SOLTO los tenedores.");
                }
            }
        }
    }

    // CLASE 3 - MAIN APPLICATION
    // para el main donde esta toda la simulacion
    public class MainApplication
    {
        public static void Main(string[] args) {
            int numberOfPhilosophers = 5;
            Fork[] forks = new Fork[numberOfPhilosophers];
            Philosopher[] philosophers = new Philosopher[numberOfPhilosophers];

            // 1. INICIAR LOS TENEDORES
            for (int i = 0; i < numberOfPhilosophers; i++) {
                forks[i] = new Fork(i);
            }

            // 2. INICIAR FILOSFOS Y DARLES TENEDORES
            for (int i = 0; i < numberOfPhilosophers; i++) {
                Fork leftFork = forks[i];
                Fork rightFork = forks[(i + 1) % numberOfPhilosophers];
                
                philosophers[i] = new Philosopher(i, leftFork, rightFork);
            }

            Console.WriteLine("--- INICIO DE LA CENA ---\n");

            // 3. INICIAR LOS HILOS SENTANDO A LOS FILOSOFOS
            foreach (var p in philosophers) {
                p.Start();
            }

            // 4. ESPERAR QUE TERMINEN DE COEMR
            foreach (var p in philosophers){
                p.Join();
            }

            Console.WriteLine("\n--- FIN DE LA CENA ---");
        }
    }
}