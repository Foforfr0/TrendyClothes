using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.Pages.Dialogs;
using WpfApp.Utilities;

namespace WpfApp.Pages.Product
{
    public partial class RegisterProductPage : Page
    {
        //private Product _editingProduct;
        private bool _isEditMode;
        private byte[] _selectedImageBytes;
        public RegisterProductPage()
        {
            InitializeComponent();
            SetInputFields();
            UpdateButtonState();
            _isEditMode = false;
        }

        /*public RegisterProductPage(Product editingProduct)
         * {
         *      
         * }
         */

        private void ConfigureInterfaceForMode()
        {
            if (_isEditMode)
            {
                PageHeader.SetResourceReference(TextBlock.TextProperty, "EditItem_Header");
                PageDescription.SetResourceReference(TextBlock.TextProperty, "EditItem_Desc");
                BtnEditProduct.Visibility = Visibility.Visible;
                BtnRegisterProduct.Visibility = Visibility.Collapsed;
            }
            else
            {
                PageHeader.SetResourceReference(TextBlock.TextProperty, "RegItem_Header");
                PageDescription.SetResourceReference(TextBlock.TextProperty, "RegItem_Desc");
                BtnEditProduct.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadProductData()
        {
            //TODO
        }

        private void SetInputFields()
        {
            var validations = new (TextBox, string, int)[]
            {
                (TbItemTitle, Constants.GENERAL_TEXT_PATTERN, Constants.MAX_LENGTH_NAMES),
                (TbItemDesc, Constants.GENERAL_TEXT_PATTERN, Constants.MAX_LENGTH_DESCRIPTION),
                (TbPrice, Constants.MONETARY_DECIMAL_PATTERN, Constants.MAX_LENGTH_MONETARY_VALUE)
            };

            foreach(var (textBox, pattern, maxLength) in validations)
            {
                InputUtilities.ValidateInput(textBox, pattern, maxLength);
            }

            InputUtilities.ValidatePriceInput(TbPrice);
        }

        private void SelectProductImage(Image targetImageControl)
        {
            var dialogTitle = Application.Current.Resources["RegItem_DialogSelectProductPic"]?.ToString();

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = Constants.IMAGE_FILE_FILTER,
                Title = dialogTitle
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var processedImageBytes = ImageUtilities.ProcessImageBeforeSaving(openFileDialog.FileName);

                    if (!ImageUtilities.IsImageSizeValid(processedImageBytes))
                    {
                        MessageDialog.Show("GlbDialogT_InvalidImageSize", "GlbDialogD_InvalidImageSize", AlertType.WARNING);
                        return;
                    }

                    _selectedImageBytes = processedImageBytes;
                    targetImageControl.Source = ImageUtilities.ConvertToImageSource(processedImageBytes);

                    BtnDeleteImage.IsEnabled = true;
                }
                catch (Exception)
                {
                    MessageDialog.Show("GlbDialogT_InvalidImageSize", "GlbDialogD_InvalidImageSize", AlertType.WARNING);
                }
            }
        }

        private void UpdateButtonState(Button button = null)
        {
            var requiredFields = new List<Control>
            {
                TbItemTitle,
                TbItemDesc,
                TbStock,
                TbPrice
            };

            bool allFieldsFilled = true;

            foreach (var field in requiredFields)
            {
                switch (field)
                {
                    case TextBox tb when string.IsNullOrWhiteSpace(tb.Text):
                        allFieldsFilled = false;
                        break;
                }

                if (!allFieldsFilled) break;
            }

            if (_isEditMode) BtnEditProduct.IsEnabled = allFieldsFilled;

            else BtnRegisterProduct.IsEnabled = allFieldsFilled;

        }

        private void RequiredFields_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonState();
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChIsUnique_Checked(object sender, RoutedEventArgs e)
        {
            //TODO set quantity to 1
            SPSizes.Visibility = Visibility.Collapsed;
        }

        private void ChIsUnique_Unchecked(object sender, RoutedEventArgs e)
        {
            SPSizes.Visibility = Visibility.Visible;
        }

        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRegisterProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.GoBack();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationManager.Instance.GoBack();
        }
    }
}
