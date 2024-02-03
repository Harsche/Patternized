using System;
using System.Collections.Generic;

namespace CommandPattern{
    public static class StackExtensions{
        public static Stack<T> Clone<T>(this Stack<T> original){
            if (original == null){ throw new ArgumentNullException(nameof(original)); }

            var array = new T[original.Count];
            original.CopyTo(array, 0);

            Array.Reverse(array);

            return new Stack<T>(array);
        }
        
        public static Stack<T> Reverse<T>(this Stack<T> original){
            return new Stack<T>(original);
        }
    }
}