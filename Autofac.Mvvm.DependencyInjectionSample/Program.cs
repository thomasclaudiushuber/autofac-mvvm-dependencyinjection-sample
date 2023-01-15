using Autofac;
using System.Collections.ObjectModel;

var builder = new ContainerBuilder();

builder.RegisterType<CustomerItemViewModel>();
builder.RegisterType<MainViewModel>();
builder.RegisterType<CustomerDataProvider>().As<ICustomerDataProvider>();

var container = builder.Build();

var mainViewModel = container.Resolve<MainViewModel>();
mainViewModel.Load();

foreach (var viewModel in mainViewModel.CustomerItemViewModels)
{
    Console.WriteLine(viewModel.Customer.FullName);
}

Console.ReadLine();

public class MainViewModel
{
    private readonly Func<Customer, CustomerItemViewModel> _customerItemViewModelCreator;
    private readonly ICustomerDataProvider _customerDataProvider;

    public MainViewModel(Func<Customer, CustomerItemViewModel> customerItemViewModelCreator,
        ICustomerDataProvider customerDataProvider)
    {
        _customerItemViewModelCreator = customerItemViewModelCreator;
        _customerDataProvider = customerDataProvider;
    }

    public void Load()
    {
        var customers = _customerDataProvider.Load();
        foreach (var customer in customers)
        {
            var customerItemViewModel = _customerItemViewModelCreator(customer);
            CustomerItemViewModels.Add(customerItemViewModel);
        }
    }

    public ObservableCollection<CustomerItemViewModel> CustomerItemViewModels { get; } = new();
}

public class CustomerItemViewModel
{
    public CustomerItemViewModel(Customer customer, ICustomerDataProvider dataProvider)
    {
        Customer = customer;
    }

    public Customer Customer { get; }
}

public class Customer
{
    public string? FullName { get; set; }
}

public interface ICustomerDataProvider
{
    IEnumerable<Customer> Load();
}

public class CustomerDataProvider : ICustomerDataProvider
{
    public IEnumerable<Customer> Load()
    {
        yield return new Customer { FullName = "Thomas" };
        yield return new Customer { FullName = "Julia" };
        yield return new Customer { FullName = "Anna" };
    }
}