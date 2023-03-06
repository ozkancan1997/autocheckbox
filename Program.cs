Test MyTest = new Test();
MyTest.UnitTest();

public class Ruleset{

    // Dependencies and Conflicts stored seperately
    public Dictionary<char, string> dependencies{get;}
    public List<string> conflicts{get;} 

    //Constructor for Ruleset data structure
    public Ruleset(){
        dependencies = new Dictionary<char, string>();
        conflicts = new List<string>();
    }

    public void AddDep(char baseOpt, char targetOpt){
        //Controls if the key is new
        if(!dependencies.Keys.Contains(baseOpt)){
            dependencies.Add(baseOpt,"");
        }
        //Prevents duplicates of dependencies
        if(!dependencies[baseOpt].Contains(targetOpt)){
            dependencies[baseOpt]+=targetOpt;
        }
        //If target has dependencies, base also has them
        if(dependencies.Keys.Contains(targetOpt)){
            foreach(char c in dependencies[targetOpt]){
                if(!dependencies[baseOpt].Contains(c)){
                    dependencies[baseOpt]+=c;
                }
            }
        }
        //If other options depend on base, they also depend on base's dependencies
        foreach(var dep in dependencies){
            if(dep.Value.Contains(baseOpt)){
                dependencies[dep.Key]+=targetOpt;
            }
        }

    }

    //Conflicts are not directed. Stored as strings
    public void AddConflict(char optA, char optB){
        char[] chars = {optA,optB};
        conflicts.Add(new string(chars));
    }

    //For printing the ruleset
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

    //Looks for conflicted and dependent options. Also looks for if
    //any option's dependencies conflicts with each other
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

    //Constructors
    public Options(){
        this.ruleset = new Ruleset();
        this.optionList = new List<char>();
    }
    public Options(Ruleset ruleset){
        this.ruleset = ruleset;
        this.optionList = new List<char>();
    }

    //Toggles an option, ıt's conflicts and dependencies.
    //Also it's dependencies's conflicts and dependencies.

    public void Toggle(char opt){
        //If the option is not selected, selects it
        if(!optionList.Contains(opt)){
            optionList.Add(opt); //Adds to the collection
            resolveConflicts(opt); //Removes its conflicts
            resolveDependencies(opt); //Also adds it's dependencies
        }else{
            removeByDependencies(opt); //If option is selected, deselects it
        }

    }

    //Returns selected options as a string
    public string StringSlice(){
        string temp="";
        foreach(char o in optionList){
            temp+=o+" ";
        }
        return temp;
    }

    //returns selected options as a char array
    public char[] Selections(){
        optionList.Sort();
        return optionList.ToArray();
    }

    //Iterates in the conflicts list and removes selected conlicted options
    private void resolveConflicts(char opt){
        foreach(string c in ruleset.conflicts){
            if(c[0]==opt && optionList.Contains(c[1])){ //if the option is the first char
                removeByDependencies(c[1]);
            }else if(c[1]==opt && optionList.Contains(c[0])){ //or the second
                removeByDependencies(c[0]);
            }
        }
    }

    //Selects the dependencies and deselects their conflicts
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

    //Deselect an option and deselects the other options that depends on that one option
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