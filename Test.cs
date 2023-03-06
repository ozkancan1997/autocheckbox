class Test{
    Ruleset rs;
    Options opts;

    bool isEqual;
    
    public void UnitTest(){
        TestDependsAA();
        TestDependsABBA();
        TestExclusiveAB();
        TestExclusiveABBC();
        TestDeepDeps();
        TestExclusiveABBCCADE();
        TestABBCToggle();
        TestABBC();
    }

    private void TestDependsAA(){
        rs = new Ruleset();
        rs.AddDep('a', 'a');

        Console.Write("True expected, ");
        if(rs.isCoherent()){
            Console.WriteLine("True found, Test 1 Passed");
            return;
        }
        Console.WriteLine("False found, Test 1 Failed");
    }

    private void TestDependsABBA(){
        rs = new Ruleset();
        rs.AddDep('a', 'b');
        rs.AddDep('b', 'a');

        Console.Write("True expected, ");
        if(rs.isCoherent()){
            Console.WriteLine("True found, Test 2 Passed");
            return;
        }
        Console.WriteLine("False found, Test 2 Failed");
    }

    private void TestExclusiveAB(){
        rs = new Ruleset();
        rs.AddDep('a', 'b');
        rs.AddConflict('a', 'b');

        Console.Write("False expected, ");
        if(rs.isCoherent()){
            Console.WriteLine("True found, Test 3 Failed");
            return;
        }
        Console.WriteLine("False found, Test 3 Passed");
    }

    private void TestExclusiveABBC(){
        rs = new Ruleset();
        rs.AddDep('a', 'b');
        rs.AddDep('b', 'c');
        rs.AddConflict('a', 'c');

        Console.Write("False expected, ");
        if(rs.isCoherent()){
            Console.WriteLine("True found, Test 4 Failed");
            return;
        }
        Console.WriteLine("False found, Test 4 Passed");
    }

    private void TestDeepDeps(){
        rs = new Ruleset();
        rs.AddDep('a', 'b');
        rs.AddDep('b', 'c');
        rs.AddDep('c', 'd');
        rs.AddDep('d', 'e');
        rs.AddDep('a', 'f');
        rs.AddConflict('e', 'f');

        Console.Write("False expected, ");
        if(rs.isCoherent()){
            Console.WriteLine("True found, Test 5 Failed");
            return;
        }
        Console.WriteLine("False found, Test 5 Passed");
    }

    private void  TestExclusiveABBCCADE(){
        rs = new Ruleset();
        rs.AddDep('a', 'b');
        rs.AddDep('b', 'c');
        rs.AddDep('c', 'a');
        rs.AddDep('d', 'e');
        rs.AddConflict('c', 'e');

        opts = new Options(rs);
        opts.Toggle('a');
        
        isEqual = opts.Selections().SequenceEqual(new char[]{'a', 'b', 'c'});

        Console.Write("abc expected, ");
        if(isEqual){
            Console.WriteLine(opts.StringSlice()+" found, Test 6 Passed");
        }else{
            Console.WriteLine(opts.StringSlice()+ " found, Test 6 Failed");
        }
        

        rs.AddDep('f', 'f');
        opts.Toggle('f');

        isEqual = opts.Selections().SequenceEqual(new char[]{'a', 'b', 'c', 'f'});

        Console.Write("abcf expected, ");
        if(isEqual){
            Console.WriteLine(opts.StringSlice()+" found, Test 7 Passed");
        }else{
            Console.WriteLine(opts.StringSlice()+ " found, Test 7 Failed");
        }

        opts.Toggle('e');
        isEqual = opts.Selections().SequenceEqual(new char[]{'e', 'f'});

        Console.Write("ef expected, ");
        if(isEqual){
            Console.WriteLine(opts.StringSlice()+" found, Test 8 Passed");
        }else{
            Console.WriteLine(opts.StringSlice()+ " found, Test 8 Failed");
        }

        opts.Toggle('b');
        isEqual = opts.Selections().SequenceEqual(new char[]{'a', 'b', 'c', 'f'});

        Console.Write("abcf expected, ");
        if(isEqual){
            Console.WriteLine(opts.StringSlice()+" found, Test 9 Passed");
        }else{
            Console.WriteLine(opts.StringSlice()+ " found, Test 9 Failed");
        }

        rs.AddDep('b','g');
        opts.Toggle('g');
        opts.Toggle('b');
        isEqual = opts.Selections().SequenceEqual(new char[]{'f', 'g'});

        Console.Write("fg expected, ");
        if(isEqual){
            Console.WriteLine(opts.StringSlice()+" found, Test 10 Passed");
        }else{
            Console.WriteLine(opts.StringSlice()+ " found, Test 10 Failed");
        }
    }

    private void TestABBCToggle(){
        rs = new Ruleset();
        rs.AddDep('a', 'b');
        rs.AddDep('b', 'c');

        opts = new Options(rs);
        opts.Toggle('c');

        isEqual = opts.Selections().SequenceEqual(new char[]{'c'});
        Console.Write("c expected, ");
        if(isEqual){
            Console.WriteLine(opts.StringSlice()+" found, Test 11 Passed");
            return;
        }
        Console.WriteLine(opts.StringSlice()+ " found, Test 11 Failed");
    }

    private void TestABBC(){
        rs = new Ruleset();
        rs.AddDep('a', 'b');
        rs.AddDep('a', 'c');
        rs.AddConflict('b', 'd');
        rs.AddConflict('b', 'e');

        Console.Write("True expected, ");
        if(rs.isCoherent()){
            Console.WriteLine("True found, Test 12 Passed");
        }else{
            Console.WriteLine("False found, Test 12 Failed");
        }
        
        opts = new Options(rs);

        opts.Toggle('d');
        opts.Toggle('e');
        opts.Toggle('a');

        isEqual = opts.Selections().SequenceEqual(new char[]{'a', 'b', 'c'});
        Console.Write("abc expected, ");
        if(isEqual){
            Console.WriteLine(opts.StringSlice()+" found, Test 13 Passed");
            return;
        }
        Console.WriteLine(opts.StringSlice()+ " found, Test 13 Failed");
    }

}