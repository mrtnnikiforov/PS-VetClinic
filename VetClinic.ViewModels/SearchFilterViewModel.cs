using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;
using VetClinic.Common;
using VetClinic.Model.Interfaces;

namespace VetClinic.ViewModels
{
    public class SearchFilterViewModel : ViewModelBase
    {
        private readonly Type _entityType;
        private readonly object _repository;
        private readonly MethodInfo _queryMethod;

        public ObservableCollection<FilterFieldInfo> FilterFields { get; }

        private ObservableCollection<object> _results = new();
        public ObservableCollection<object> Results
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        private string _notFoundMessage = string.Empty;
        public string NotFoundMessage
        {
            get => _notFoundMessage;
            set
            {
                if (SetProperty(ref _notFoundMessage, value))
                {
                    OnPropertyChanged(nameof(HasNotFoundMessage));
                }
            }
        }

        public bool HasNotFoundMessage => !string.IsNullOrWhiteSpace(NotFoundMessage);

        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }

        public SearchFilterViewModel(Type entityType, object repository)
        {
            _entityType = entityType;
            _repository = repository;

            _queryMethod = repository.GetType().GetMethod("Query")
                ?? throw new InvalidOperationException("Repository must have a Query method");

            FilterFields = new ObservableCollection<FilterFieldInfo>(
                ReflectionHelper.GetSearchableFields(entityType));

            SearchCommand = new RelayCommand(_ => ExecuteSearch());
            ClearCommand = new RelayCommand(_ => ClearFilters());
        }

        private void ExecuteSearch()
        {
            NotFoundMessage = string.Empty;

            var parameter = Expression.Parameter(_entityType, "e");
            Expression? combinedFilter = null;

            foreach (var field in FilterFields)
            {
                if (field.Value == null)
                    continue;

                var valueStr = field.Value.ToString();
                if (string.IsNullOrWhiteSpace(valueStr))
                    continue;

                var property = Expression.Property(parameter, field.PropertyName);
                Expression? condition = null;

                if (field.PropertyType == typeof(string))
                {
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                    var constant = Expression.Constant(valueStr);
                    condition = Expression.Call(property, containsMethod, constant);
                }
                else if (field.PropertyType == typeof(DateTime))
                {
                    if (DateTime.TryParse(valueStr, out var dateValue))
                    {
                        var dateProperty = Expression.Property(property, "Date");
                        var constant = Expression.Constant(dateValue.Date);
                        condition = Expression.GreaterThanOrEqual(dateProperty, constant);
                    }
                }
                else if (field.PropertyType == typeof(int) || field.PropertyType == typeof(double) || field.PropertyType == typeof(decimal))
                {
                    try
                    {
                        var converted = Convert.ChangeType(valueStr, field.PropertyType);
                        var constant = Expression.Constant(converted);
                        condition = Expression.Equal(property, constant);
                    }
                    catch { }
                }
                else if (field.PropertyType.IsEnum)
                {
                    if (Enum.TryParse(field.PropertyType, valueStr, true, out var enumValue))
                    {
                        var constant = Expression.Constant(enumValue);
                        condition = Expression.Equal(property, constant);
                    }
                }

                if (condition != null)
                {
                    combinedFilter = combinedFilter == null
                        ? condition
                        : Expression.AndAlso(combinedFilter, condition);
                }
            }

            if (combinedFilter == null)
            {
                var getAllMethod = _repository.GetType().GetMethod("GetAll")!;
                var allResults = (System.Collections.IList)getAllMethod.Invoke(_repository, null)!;
                Results = new ObservableCollection<object>(allResults.Cast<object>());
            }
            else
            {
                var lambdaType = typeof(Expression<>).MakeGenericType(
                    typeof(Func<,>).MakeGenericType(_entityType, typeof(bool)));

                var lambda = Expression.Lambda(combinedFilter, parameter);
                var results = (System.Collections.IList)_queryMethod.Invoke(_repository, new object[] { lambda })!;
                Results = new ObservableCollection<object>(results.Cast<object>());
            }

            if (Results.Count == 0)
            {
                NotFoundMessage = "No results!";
            }
        }

        private void ClearFilters()
        {
            foreach (var field in FilterFields)
            {
                field.Value = null;
            }
            Results.Clear();
            NotFoundMessage = string.Empty;
        }
    }
}
