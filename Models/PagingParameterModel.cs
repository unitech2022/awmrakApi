namespace awamrakeApi.Models
{
    public class PagingParameterModel
    {
          
      
        private const int _maxItemsPerPage = 50;
        private int itemsPerPage;

        public int Page { get; set; } = 1;
        public int ItemsPerPage 
        { 
            get => itemsPerPage; 
            set => itemsPerPage = value > _maxItemsPerPage ? _maxItemsPerPage : value; 
        }
    }
    
    }
