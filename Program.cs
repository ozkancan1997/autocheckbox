using System.Collections.Generic;

Ruleset rs = new Ruleset();

// rs.AddDep('A', 'B');
// rs.AddDep('C', 'A');

rs.AddDep('A', 'B');
rs.AddDep('B', 'C');
rs.AddDep('C', 'D');
rs.AddDep('D','B');
rs.AddConflict('A','C');

Console.WriteLine(rs);

Console.WriteLine(rs.isCoherent());


public class Ruleset{
    Dictionary<char, string> dependencies;
    List<string> conflicts;
    string a="";

    public Ruleset(){
        dependencies = new Dictionary<char, string>();
        conflicts = new List<string>();
    }

    public void AddDep(char baseOpt, char targetOpt){
        if(!dependencies.Keys.Contains(baseOpt)){
            dependencies.Add(baseOpt,"");
        }
        dependencies[baseOpt]+=targetOpt;
        if(dependencies.Keys.Contains(targetOpt)){
            dependencies[baseOpt]+=dependencies[targetOpt];
        }
        foreach(var dep in dependencies){
            if(dep.Value.Contains(baseOpt)){
                dependencies[dep.Key]+=dependencies[baseOpt];
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
            if(dependencies[c[0]].Contains(c[1]) || dependencies[c[1]].Contains(c[0])){
                return false;
            }
        }
        return true;
    }
}