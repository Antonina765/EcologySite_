using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;
using Ecology.Data.Models.Ecology;
using Ecology.Data.DataLayerModels;
using Ecology.Data.Models.SqlRawModels;
using Enums;
using Enums.Ecology;
using Enums.Users;
using Microsoft.EntityFrameworkCore;

namespace Ecology.Data.Repositories;

public interface IEcologyRepositoryReal : IEcologyRepository<EcologyData>
{
    void Create(EcologyData ecology, int currentUserId, int postId);
    IEnumerable<EcologyData>GetAllWithUsersAndComments();
    Pagginator<EcologyData> GetAllWithUsersAndComments(int page, int perPage, string fieldNameForSort, OrderDirection orderDirection);
    void SetForMainPage(Type postId);
    IEnumerable<PostWithMainStatus> GetPostsForMainPage();
    bool LikeEcology(int ecologyId, int userId);
}

public class EcologyRepository : BaseRepository<EcologyData>, IEcologyRepositoryReal
{
    public EcologyRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public void UpdatePost(int id, string url, string text)
    {
        var ecology = _dbSet.First(e => e.Id == id); 

        ecology.ImageSrc = url;
        ecology.Text = text; 
                
        _webDbContext.SaveChanges();
    }

    public void SetForMainPage(Type postId)
    {
        var ecology = _dbSet.Find(postId);
        if (ecology != null ) //(ecology != null && Role == "Admin")
        {
            ecology.ForMainPage = 1; 
            _webDbContext.SaveChanges();
        }
    }
    
    public Pagginator<EcologyData> GetAllWithUsersAndComments(
        int page,
        int perPage,
        string fieldNameForSort,
        OrderDirection orderDirection)
    {
        var items = _dbSet
            .Include(x => x.User)
            .Include(x => x.Comments)
            .Include(x => x.UsersWhoLikeIt)
            .AsQueryable();

       /* switch (sortType)
        {
            case EcologySortType.Like:
                items = items.OrderBy(x => x.UsersWhoLikeIt.Count);
                break;
            case EcologySortType.Default:
                items = items.OrderBy(x => x.Id);
                break;
        }*/ 
       items = SortAndGetAll(items, fieldNameForSort);
       
       if (orderDirection == OrderDirection.Desc)
       { 
           items = items.OrderByDescending(x => x.Id); 
       }

        var data = new Pagginator<EcologyData>();
        data.TotalRecords = items.Count();
        data.Items = items.Skip((page - 1) * perPage)
            .Take(perPage)
            .ToList();
        return data;
    }

    public IEnumerable<EcologyData> GetAllWithUsersAndComments()
    {
        return _dbSet
            .Include(x => x.User)
            .Include(x => x.Comments)
            .Include(x => x.UsersWhoLikeIt)
            .ToList();
    }
   
    public void Create(EcologyData ecology, int currentUserId, int postId)
    {
        var creator = _webDbContext.Users.FirstOrDefault(x => x.Id == currentUserId);
        
        var comments = _webDbContext.Comments.Where(x => x.Id == postId);

        ecology.User = creator;
        ecology.Comments = comments;

        Add(ecology);
    }
    
    public bool LikeEcology(int ecologyId, int userId)
    {
        var ecology = _dbSet
            .Include(x => x.UsersWhoLikeIt)
            .First(x => x.Id == ecologyId);
        var user = _webDbContext.Users.First(x => x.Id == userId);

        var isUserAlreadyLikeTheEcology = ecology
            .UsersWhoLikeIt
            .Any(ue => ue.UserId == userId);

        if (isUserAlreadyLikeTheEcology)
        {
            var userEcology = ecology.UsersWhoLikeIt.First(ue => ue.UserId == userId);
            ecology.UsersWhoLikeIt.Remove(userEcology); // Удаляем объект UserEcologyLikesData
            _webDbContext.SaveChanges();
            return false;
        }

        ecology.UsersWhoLikeIt
            .Add(new UserEcologyLikesData
            {
                UserId = userId,
                EcologyDataId = ecologyId, 
                User = user
            });
        _webDbContext.SaveChanges();
        return true;
    }
    
    // Метод для получения постов с информацией о статусе
    public IEnumerable<PostWithMainStatus> GetPostsForMainPage() 
    {
        var sql = @" 
            SELECT 
                P.""Id"", 
                P.""Text"", 
                P.""ImageSrc"", 
                P.""ForMainPage"", 
                CASE WHEN P.""ForMainPage"" = 1 THEN 'ForMainPage' ELSE 'NotForMainPage' END as ""MainPageStatus"" 
            FROM ""Ecologies"" P";
        // Выполнение запроса и получение данных
        var result = _webDbContext
            .Database
            .SqlQueryRaw<PostWithMainStatus>(sql)
            .ToList(); 
        return result; 
    }
}