using System.Collections;

namespace EcologySite.Models.Ecology;

public class PostViewModel
{
    public PagginatorViewModel<EcologyViewModel> Ecologies { get; set; }
    
    public List<PostCreationViewModel> Posts { get; set; }
}