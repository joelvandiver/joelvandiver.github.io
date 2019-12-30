#include <iostream>
#include <vector>
#include <string>

using namespace std;

void printfn(string line)
{
    cout << line;
    cout << endl;
}

void printNums(int a, int b, int c)
{
    printfn("| " + 
        to_string(a) + " | " + 
        to_string(b) + " | " + 
        to_string(c) + " |");
}

int multiply_by_adding_recursively(int n, int a)
{
    if (n == 1) return a;
    const int total = multiply_by_adding_recursively(n - 1, a) + a;
    printNums(n, a, total);
    return total;
}

void egyptian()
{
    printfn("# Egyptian Mutliplication");
    printfn("> Multiplying 8 x 12 by Adding 8 12s");
    printfn("| 8 | 12 |    |");
    printfn("| - | -- | -- |");
    const int x = multiply_by_adding_recursively(8, 12);
    cout << "8 x 12 = ";
    cout << x;
}

int main()
{
    egyptian();
}