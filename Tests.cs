using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Xunit;

namespace Array2Tests
{
    public class Array2Tests
    {
        private IEnumerable<string> shortArray;
        private IEnumerable<string> middleArray;
        private IEnumerable<string> longArray;
        private string newItem = "newItem";

        public Array2Tests()
        {
            Fixture autoFixture = new Fixture();
            #region FSetup

            autoFixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => autoFixture.Behaviors.Remove(b));
            autoFixture.Behaviors.Add(new OmitOnRecursionBehavior());

            #endregion
            shortArray = autoFixture.CreateMany<string>(1);
            middleArray = autoFixture.CreateMany<string>(500000);
            longArray = autoFixture.CreateMany<string>(1000000);
        }
        
        [Fact]
        public async Task Queue_CleanAll()
        {
            var shortQueue = new Queue<string>(shortArray);
            DequeueAll(shortQueue);
            
            var middleQueue = new Queue<string>(middleArray);
            DequeueAll(middleQueue);
            
            var longQueue = new Queue<string>(longArray);
            DequeueAll(longQueue);
        }
        
        [Fact]
        public async Task Queue_AddElement()
        {
            var shortQueue = new Queue<string>(shortArray);
            AddElementToQueue(shortQueue, newItem);
            
            var middleQueue = new Queue<string>(middleArray);
            AddElementToQueue(middleQueue, newItem);
            
            var longQueue = new Queue<string>(longArray);
            AddElementToQueue(longQueue, newItem);
        }
        
        [Fact]
        public async Task Stack_CleanAll()
        {
            var shortStack = new Stack<string>(shortArray);
            CleanAll(shortStack);
            
            var middleStack = new Stack<string>(middleArray);
            CleanAll(middleStack);
            
            var longStack = new Stack<string>(longArray);
            CleanAll(longStack);
        }
        
        [Fact]
        public async Task Stack_AddElement()
        {
            var shortStack = new Stack<string>(shortArray);
            AddElementToStack(shortStack, newItem);
            
            var middleStack = new Stack<string>(middleArray);
            AddElementToStack(middleStack, newItem);
            
            var longStack = new Stack<string>(longArray);
            AddElementToStack(longStack, newItem);
        }

        private void DequeueAll(Queue<string> queue)
        {
            RunWithStopwatch(() =>
            {
                while (queue.Count != 0)
                {
                    queue.Dequeue();    
                }
            });
        }
        
        private void CleanAll(Stack<string> stack)
        {
            RunWithStopwatch(() =>
            {
                while (stack.Count != 0)
                {
                    stack.Pop();    
                }
            });
        }

        private void AddElementToQueue(Queue<string> queue, string newElement)
        {
            RunWithStopwatch(() =>
            {
                queue.Enqueue(newElement);
            });
        }
        
        private void AddElementToStack(Stack<string> stack, string newElement)
        {
            RunWithStopwatch(() =>
            {
                stack.Push(newElement);
            });
        }
        
        private void RunWithStopwatch(Action action)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}