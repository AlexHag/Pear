using SQLite;
using Pear.Client.Models;

namespace Pear.Client.Repositories;

public class PearFriendsRepository
{
    private readonly SQLiteConnection _conn;

    public PearFriendsRepository(string dbPath)
    {
        _conn = new SQLiteConnection(dbPath);
        _conn.CreateTable<PearFriend>();
    }

    public int Add(PearFriend pearFriend)
    {
        int result = _conn.Insert(pearFriend);
        return result;
    }

    public void Delete(int id)
    {
        _conn.Delete<PearFriend>(id);
    }

    public List<PearFriend> GetAll()
    {
        List<PearFriend> pearFriends = _conn.Table<PearFriend>().ToList();
        return pearFriends;
    }

    public PearFriend GetByName(string name)
    {
        var pearFriend = from f in _conn.Table<PearFriend>()
                         where f.Name == name
                         select f;
        return pearFriend.FirstOrDefault();
    }
}
