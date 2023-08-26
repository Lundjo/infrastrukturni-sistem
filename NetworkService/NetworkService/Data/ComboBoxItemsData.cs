using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Data
{
	public class ComboBoxItemsData
	{
		public static Dictionary<string, string> entityTypes = new Dictionary<string, string>()
		{
			{ "Interval", "pack://application:,,,/NetworkService;component/Images/SmartMeter.png" },
			{ "Smart", "pack://application:,,,/NetworkService;component/Images/IntervalMeter.png" }
		};
	}
}
