namespace MriBase.Models.Models
{
    public class MyTuple<T, U>
    {
        public MyTuple(T item1, U item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public T Item1 { get; set; }
        public U Item2 { get; set; }
    }
}
