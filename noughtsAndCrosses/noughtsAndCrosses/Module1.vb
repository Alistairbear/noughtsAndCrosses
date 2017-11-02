Module Module1

    ' honestly this is one of the best annotated peices of code I've ever written, look back on this for inspiration on how to annotate well when writing code.
    ' Would you have guessed the secret is to annotate as you go and try to annotate every single line unless it is blindingly obvious what it does, to the point where even someone who doesn't know the language can easily understand it.

    Sub Main()
        Dim name(1) As String ' name is a one dimensional array that stores the names of the players
        Dim answer As String = String.Empty ' answer is more of a temporary variable for inputs to aid with error checking
        Dim valid As Boolean = False ' valid is a booleam variable used when error checking inputs
        For i As Integer = 0 To 1 ' for loop is used to stop repeting code, ironically uses the same number of lines but if for some reason you wanted to change the game to be 3 player, then it's much easier
            Console.WriteLine("enter the name for player " & i + 1) ' i + 1 is used because of zero indexing with the array
            name(i) = StrConv(Console.ReadLine(), vbProperCase) ' no error checking needed because name is a string and is only used to refer to the player not for anything technical
        Next
        Console.WriteLine("Welcome to the program " & name(0) & " and " & name(1)) ' a lovely welcome message. Also hello to you, person looking at my code!
        Do
            Console.WriteLine("please choose either 1) play game 2) load a win 3) save a win") ' wish I could use a more elegant approach to the menu but the brief specifically mentions this method so that's the method I'm using
            answer = Console.ReadLine()
            Select Case answer
                Case "1", "2", "3" ' seeing if the input is one of the three valid options, if so then it is valid otherwise it is not
                    valid = True 'once again using the valid boolean variable to error check the input
            End Select
        Loop Until valid ' The do loop loops the input request until a valid input has been entered. No need to check if the input is an integer because we're looking for specific values. If the brief wants me to code it worse then I will but hey what're you gonna do?
        Select Case answer ' we know at this stage that answer must be one of the three options but in the weird off chance it's not then it won't matter anyway because it just won't call any of the subprocesses
            Case "1" ' so these case statements are the ones used to call the correct subprocess based on the user's input.
                playGame()
            Case "2"
                loadAWin()
            Case "3"
                saveAWin()
        End Select
        Console.ReadKey()
    End Sub

    'play subprocesses

    Sub playGame()
        Dim finished As Boolean = False
        Dim symbols(1) As Char ' symbols is a one dimentional array used to store the single character symbols that represent the players on the board hence why it can be char instead of string
        Dim board(2, 2) As Char ' board is a two dimentional array that stores the game board in it's current state.
        Console.WriteLine("you are now in the play game subprocess") ' purely used to check whether the program is reaching this point
        Do
            symbols = {pickSymbols(1), pickSymbols(2)} ' declaring symbols() by assigning each element to the returned value from the function picksymbols
            If symbols(0) = symbols(1) Then
                Console.WriteLine("the symbols must be different")
            End If
        Loop Until symbols(0) <> symbols(1) ' error checking to make sure the symbols aren't the same

        For i As Integer = 0 To 2 ' does it need to be said that it's 0 to 2 because of zero indexing on the board array? Possibly not, but it has been said now so that is that.
            For j As Integer = 0 To 2
                board(i, j) = "-" ' filling the board with empty slots
            Next
        Next
        While Not finished
            For i As Integer = 0 To 1
                Console.WriteLine("player " & i + 1 & "'s turn")
                updateGameGrid(board)
                board = placeSymbol(board, symbols(i))
            Next
        End While
    End Sub

    Function pickSymbols(player As Integer) As Char ' the argument in this function is purely asthetic so doesn't matter too much. It's not used for anything other than a console.writeline
        Dim input As String ' declared as a string to aid with error checking. if a player enters more than one character into a char it would crash but a string wouldn't
        Do
            Console.WriteLine("please choose a single, unique character symbol to represent player " & player) ' this is what I mean when I say player is purely asthetic because this is the only line that uses it.
            input = Console.ReadLine()
        Loop Until input.Length = 1 'Keeps looping the input until the string is one character long so can be accepted as a char variable type
        Return input ' returns the now single character string. This could be problematic. Say input somehow becomes more than one character, the program would crash because a char cannot be more than 1 character
    End Function

    Sub updateGameGrid(grid(,) As Char) ' the fact that grid has no parameters means there could be an index out of range error when the program runs through the loops
        Console.Clear() ' clearing the console for the new round
        For i As Integer = 0 To 3 ' bit of a mess here because of zero indexing and the fact that the board would need to be shifted along one place in both x and y
            For j As Integer = 0 To 3
                If (i <> 0) And (j = 0) Then 'This is for the first row
                    Console.Write(i) ' outputting i because i can be different and still meet these parameters
                ElseIf (i = 0) And (j <> 0) Then ' this is for the first column
                    Console.Write(j) ' outputting j because j can be different and still meet these parameters
                ElseIf (i = 0) And (j = 0) Then ' This is for when the first row and the first column cross and so need to be blank
                    Console.Write(" ") ' a space is used rather than an empty string because an empty string would just not write out anything whereas a space actually produces a blank space.
                Else
                    Console.Write(grid((i - 1), (j - 1))) ' If not the first row/column then it needs to draw the board, potential error here if i or j are < 1 then program will crash so it's risky but shouldn't break simply due to the parameters of the if statements. Needs to be (i - 1) because, you guessed it, zero indexing
                End If
                Console.Write(" ")
            Next
            Console.WriteLine()
        Next
    End Sub

    Function placeSymbol(grid(,) As Char, symbol As Char) As Char(,) ' this function might be better suited as a subprocess because it's only trying to change grid and I believe that's what ByRef is for? can't rememeber. This function has subprocess dysphoria.
        Dim coordinates(1) As Integer ' coordiantes is an array with 2 elements and is used to store the coordinates of the position that the player wants to place their counter in. The first element is the x coordiante and the second element is the y coordinate just like standard coordinate notation.
        Dim input As String ' input is used to error check; make sure that the user enters a number without crashing the program
        Do
            Do
                Console.WriteLine("What is the x coordinate of the position you would like to place your counter in?") ' this block (in fact this entire function tbh) is messy and frankly a bit crap so I will probably rewrite this at some point if I feel like it matters enough to be re written
                input = Console.ReadLine()
            Loop Until IsNumeric(input) ' inner do loop is used to check whether the input is actually a number
            coordinates(1) = input
            coordinates(1) -= 1
            'the program needs to only accept positive integers within the bounds of the grid, one solution would be more recursive do loops but that's not very elegant.
            ' maybe a select case (or an if) within a loop? as in select case i. that could be quite nice
            Do
                Console.WriteLine("What is the y coordinate of the position you would like to place your counter in?") ' this whole do loop can be seen as an almost identical copy of the do loop directly above this and though that means there is repetition in my code, it is hard code to turn into a loop and doesn't matter a huge amount because it's only repeated once not like 10 times or something
                input = Console.ReadLine()
            Loop Until IsNumeric(input)
            coordinates(0) = input
            coordinates(0) -= 1
        Loop Until grid(coordinates(0), coordinates(1)) = "-" ' looping until they enter a position which is empty. unsure whether this would return an index out of range error

        grid(coordinates(0), coordinates(1)) = symbol
        Return grid
    End Function


    'load a win subprocesses [NEED TO BE ANNOTATED]

    Sub loadAWin()
        Console.WriteLine("you are now in the load a win subprocess")
    End Sub


    'save a win subprocesses [NEED TO BE ANNOTATED]

    Sub saveAWin()
        Console.WriteLine("you are now in the save a win subprocess")
    End Sub

End Module
