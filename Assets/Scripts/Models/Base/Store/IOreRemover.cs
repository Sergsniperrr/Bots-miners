using System.Collections.Generic;

public interface IOreRemover
{
    bool TryRemoveOres(Dictionary<string, int> requireOres);
}
