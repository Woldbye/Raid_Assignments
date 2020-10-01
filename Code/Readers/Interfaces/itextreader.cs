namespace Readers 
{ 
  interface ITextReader<T>
  {
    // should be protected, but not allowed in c# 7 :)
    void read(T lines);
  }
}