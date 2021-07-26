using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSRGenerator.ViewModels
{
    public class FileOpenDialogModel
    {

        public List<string> FileExtensionsFilter { get; set; } = new List<string>();

        public string FilterLabel { get; set; } = "";

        public string? SelectedFile { get; set; } = null;

    }
}
