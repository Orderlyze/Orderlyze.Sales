using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoApp.Navigation;

internal interface INavigationAware<T>
{
    void OnNavigatedTo(T e);
    void OnNavigatedFrom(T e);
}
