using System.Collections.Generic;

Ruleset rs = new Ruleset();

// Unit Test 1
// rs.AddDep('a', 'b');
// rs.AddDep('b', 'c');
// rs.AddDep('c', 'a');
// rs.AddDep('d', 'e');
// rs.AddConflict('c', 'e');

// Options ops = new Options(rs);

// ops.Toggle('a');
// ops.StringSlice();

// rs.AddDep('f', 'f');
// ops.Toggle('f');
// ops.StringSlice();

// ops.Toggle('e');
// ops.StringSlice();

// ops.Toggle('b');
// ops.StringSlice();

// rs.AddDep('b','g');
// ops.Toggle('g');
// ops.Toggle('b');
// ops.StringSlice();

// Unit Test 2

// rs.AddDep('a', 'b');
// rs.AddDep('a', 'c');

// rs.AddConflict('b', 'd');
// rs.AddConflict('b', 'e');

// Console.WriteLine(rs.isCoherent());

// Options ops = new Options(rs);

// ops.Toggle('d'); ops.Toggle('e'); ops.Toggle('a');
// ops.StringSlice();

// Unit Test 3
// rs.AddDep('a', 'b');
// rs.AddDep('b', 'c');

// Options opts = new Options(rs);
// opts.Toggle('c');
// opts.StringSlice();

public class Ruleset{

    public Dictionary<char, string> dependencies{get;}
    public List<string> conflicts{get;}
    string a="";

    public Ruleset(){
        dependencies = new Dictionary<char, string>();
        conflicts = new List<string>();
    }

    public void AddDep(char baseOpt, char targetOpt){
        if(!dependencies.Keys.Contains(baseOpt)){
            dependencies.Add(baseOpt,"");
        }
        if(!dependencies[baseOpt].Contains(targetOpt)){
            dependencies[baseOpt]+=targetOpt;
        }
        if(dependencies.Keys.Contains(targetOpt)){
            foreach(char c in dependencies[targetOpt]){
                if(!dependencies[baseOpt].Contains(c)){
                    dependencies[baseOpt]+=c;
                }
            }
        }
        foreach(var dep in dependencies){
            if(dep.Value.Contains(baseOpt)){
                dependencies[dep.Key]+=targetOpt;
            }
        }

    }

    public void AddConflict(char optA, char optB){
        char[] chars = {optA,optB};
        conflicts.Add(new string(chars));
    }

    public override string ToString(){
        string temp="";

        foreach(var dep in dependencies){
            temp+= dep.Key + " " + dep.Value +"\n";
        }
        temp+="\n";

        foreach(string s in conflicts){
            temp+= s +"\n";
        }

        return temp;
    }

    public bool isCoherent(){
        foreach(string c in conflicts){
            if(dependencies.Keys.Contains(c[0]) && dependencies[c[0]].Contains(c[1])){
                return false;
            }else if(dependencies.Keys.Contains(c[1]) && dependencies[c[1]].Contains(c[0])){
                return false;
            }
            foreach(string v in dependencies.Values){
                if(v.Contains(c[0]) && v.Contains(c[1])){
                    return false;
                }
            }
        }
        
        return true;
    }

}

public class Options{
    Ruleset ruleset;
    List<char> optionList;

    char prev;

    public Options(){
        this.ruleset = new Ruleset();
        this.optionList = new List<char>();
    }
    public Options(Ruleset ruleset){
        this.ruleset = ruleset;
        this.optionList = new List<char>();
    }

    public void Toggle(char opt){
        if(!optionList.Contains(opt)){
            optionList.Add(opt);
            resolveConflicts(opt);
            resolveDependencies(opt);
        }else{
            removeByDependencies(opt);
        }

    }

    public string StringSlice(){
        string temp="";
        foreach(char o in optionList){
            temp+=o+" ";
        }
        Console.WriteLine(temp+"\n");
        return temp;
    }

    private void resolveConflicts(char opt){
        foreach(string c in ruleset.conflicts){
            if(c[0]==opt && optionList.Contains(c[1])){
                removeByDependencies(c[1]);
            }else if(c[1]==opt && optionList.Contains(c[0])){
                removeByDependencies(c[0]);
            }
        }
    }

    private void resolveDependencies(char opt){

        if(ruleset.dependencies.Keys.Contains(opt)){
            foreach(char d in ruleset.dependencies[opt]){
                if(!optionList.Contains(d)){
                    optionList.Add(d);
                    resolveConflicts(d);
                }  
            }
        }
    }

    private void removeByDependencies(char opt){

        if(optionList.Contains(opt)){
        optionList.Remove(opt);
        foreach(var dep in ruleset.dependencies){
            if(dep.Value.Contains(opt)){
                optionList.Remove(dep.Key);
            }
        }            
        }
    }
}