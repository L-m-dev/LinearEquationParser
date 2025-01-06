using System;
using System.Collections;

int matrixLengthConfig = 16;
int numberWidthConfig = 2;

Calculate("1x+5");
Calculate("3x-5");
Calculate("1x+2");

void Calculate(string formula) {

    Dictionary<int, int> coordinates = ApplyFunction(formula);

    #region
    //creating a dict with Y as keys and a  list of X values
    //various X values are associated with a single Y
    Dictionary<int, List<int>> yKeyWithListOfAssociatedX = GroupCoordinatesByY(coordinates);
    #endregion


    //from 15 to 0.
    for (int i = matrixLengthConfig - 1; i >= 0; i--) {
        if (i == 0) {
            for (int j = 0; j < matrixLengthConfig; j++) {

                Console.Write(j.ToString().PadLeft(2, '0'));
                for (int k = 1; k > 0; k--) {
                    Console.Write(" ");
                }
               
            }
            Console.WriteLine(); 
            Console.WriteLine();

            break;
        }

        if (yKeyWithListOfAssociatedX.ContainsKey(i)) {
            int lastPositionPointer = 0;
            bool processedZero = false;
            bool printedNumberLineItem = false;

            foreach (var item in yKeyWithListOfAssociatedX[i]) {
                if (item == 0 && !processedZero) {

                    Console.Write("()");
                    printedNumberLineItem = true;
                    processedZero = true;
                    continue;
                }
                else {

                    if (!printedNumberLineItem) {
                        Console.Write(i.ToString().PadLeft(2, '0'));
                        printedNumberLineItem = true;
                    }
                    DrawEmptyBlocksChar(item - lastPositionPointer - 1, ' ');
                    //leading space
                    Console.Write(" ");
                    Console.Write("()");
                    lastPositionPointer = item;
                }
            }
            Console.WriteLine();
        }

        else {
            Console.WriteLine(i.ToString().PadLeft(2, '0'));
        }
    }
}

Dictionary<int, List<int>> GroupCoordinatesByY(Dictionary<int, int> coordinates) {
    if (coordinates.Count < 1) {
        throw new ArgumentException("No coordinates provided.");
    }
    Dictionary<int, List<int>> yKeyWithListOfAssociatedX = new Dictionary<int, List<int>>();

    foreach (var pair in coordinates) {
        List<int> xValueList = new();

        if (yKeyWithListOfAssociatedX.ContainsKey(pair.Value)) {
            xValueList = yKeyWithListOfAssociatedX[pair.Value];
            xValueList.Add(pair.Key);
            yKeyWithListOfAssociatedX[pair.Value] = xValueList;
        }
        else {
            xValueList = new List<int>();
            xValueList.Add(pair.Key);
            yKeyWithListOfAssociatedX.Add(pair.Value, xValueList);
        }
    }
    return yKeyWithListOfAssociatedX;
}

void DrawEmptyBlocksChar(int length, char character) {
    int charCounter = 0;
    while (charCounter < length) {
        Console.Write(" ");
        int widthCounter = numberWidthConfig;
        while (widthCounter > 0) {
            Console.Write(character);
            widthCounter--;
        }
        charCounter++;
    }
}

Dictionary<int, int> ApplyFunction(string args) {
    // x = y
    List<string> parsedArguments = Parser(args);

    Console.WriteLine($"Equation: y = {parsedArguments[1]}x {parsedArguments[0]} {parsedArguments[2]}");

    Dictionary<int, int> coordinates = new();
    int argA = Int32.Parse(parsedArguments[1]);
    int argB = Int32.Parse(parsedArguments[2]);

    for (int i = 0; i < matrixLengthConfig; i++) {
        int result = 0;
        if (parsedArguments[0].Equals("+")) {
            result = (argA * i) + argB;
        }
        if (parsedArguments[0].Equals("-")) {
            result = (argA * i) - argB;
        }
        coordinates.Add(i, result);
    }
    return coordinates;
}

static List<string> Parser(string args) {
    args = args.Trim();
    int operationIndex = args.IndexOfAny(new char[] { '-', '+' });
    if (operationIndex < 0) {
        throw new ArgumentException("Argument Exception. Should contain a supported mathematical operation. Input received:"+
            args);
    }
    string operation = args.Substring(operationIndex, 1);
    int xIndex = args.IndexOf("x");

    if (!Int32.TryParse(args.Substring(0, xIndex), out int argA)) {
        throw new ArgumentException("Error: Invalid argument A.");

    }
    if (!Int32.TryParse(args.Substring(operationIndex + 1, args.Length - operationIndex - 1), out int argB)) {
        throw new ArgumentException("Error: Invalid argument B.");
    }

    return new List<string> { operation, argA.ToString(), argB.ToString() };
}

//int GetNumber() {
//    return 3;
//}

//Assert.AreEqual(5, GetNumber());
