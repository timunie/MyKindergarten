using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace MyKindergarten.Controls
{
    public class IntellisenseTextBox : TextBox
    {
        // Templateparts
        Popup PART_IntellisensePopup;
        ListBox PART_IntellisenseListBox;

        // Using a DependencyProperty as the backing store for ContentAssistSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentAssistSourceProperty = 
            DependencyProperty.Register("ContentAssistSource", typeof(IEnumerable<string>), typeof(IntellisenseTextBox), new UIPropertyMetadata(new List<string>(), OnContentAssistSourceChanged));

        public IEnumerable<string> ContentAssistSource
        {
            get { return (IEnumerable<string>)GetValue(ContentAssistSourceProperty); }
            set { SetValue(ContentAssistSourceProperty, value); }
        }

        public ICollectionView ContentAssistSource_CollectionView { get; private set; }

        private static void OnContentAssistSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IntellisenseTextBox intellisenseTextBox)
            {
                intellisenseTextBox.ContentAssistSource_CollectionView = CollectionViewSource.GetDefaultView(e.NewValue ?? new List<string>()) ;
                intellisenseTextBox.ContentAssistSource_CollectionView.Filter = o => { return ((string)o).Contains(intellisenseTextBox.sbLastWords.ToString(), StringComparison.OrdinalIgnoreCase); }; 
            }
        }

        // Using a DependencyProperty as the backing store for SuffixAfterInsert.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuffixAfterInsertProperty =
            DependencyProperty.Register("SuffixAfterInsert", typeof(string), typeof(IntellisenseTextBox), new PropertyMetadata(null));
        public string SuffixAfterInsert
        {
            get { return (string)GetValue(SuffixAfterInsertProperty); }
            set { SetValue(SuffixAfterInsertProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_IntellisensePopup = (Popup)GetTemplateChild(nameof(PART_IntellisensePopup));
            this.PART_IntellisenseListBox = (ListBox)GetTemplateChild(nameof(PART_IntellisenseListBox));

            this.PART_IntellisenseListBox.MouseDoubleClick += PART_IntellisenseListBox_MouseDoubleClick;
            this.PART_IntellisenseListBox.PreviewKeyDown += PART_IntellisenseListBox_PreviewKeyDown;
        }

        private void PART_IntellisenseListBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //if Enter\Tab\Space key is pressed, insert current selected item to richtextbox
            if (e.Key == Key.Enter || e.Key == Key.Tab || e.Key == Key.Space)
            {
                InsertAssistWord();
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                //Baskspace key is pressed, set focus to richtext box
                if (sbLastWords.Length >= 1)
                {
                    sbLastWords.Remove(sbLastWords.Length - 1, 1);
                }
                this.Focus();
            }
        }

        private void PART_IntellisenseListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            InsertAssistWord();
        }


        #region Content Assist
        private bool IsAssistKeyPressed = false;
        private StringBuilder sbLastWords = new System.Text.StringBuilder();

        private bool InsertAssistWord()
        {
            bool isInserted = false;
            if (PART_IntellisenseListBox.SelectedIndex != -1)
            {
                string selectedString = ((string)PART_IntellisenseListBox.SelectedItem).Remove(0, sbLastWords.Length);
                selectedString += SuffixAfterInsert;

                this.InsertText(selectedString);

                isInserted = true;
            }

            PART_IntellisensePopup.IsOpen = false;
            sbLastWords.Clear();
            IsAssistKeyPressed = false;
            return isInserted;
        }

        public void InsertText(string text)
        {
            Focus();
            Text = Text.Insert(CaretIndex, text);
            CaretIndex += text.Length;
        }


        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!IsAssistKeyPressed)
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            ContentAssistSource_CollectionView.Refresh();

            if (e.Key == System.Windows.Input.Key.Back)
            {
                if (sbLastWords.Length > 0)
                {
                    sbLastWords.Remove(sbLastWords.Length - 1, 1);
                    ContentAssistSource_CollectionView.Refresh();
                }
                else
                {
                    IsAssistKeyPressed = false;
                    sbLastWords.Clear();
                    PART_IntellisensePopup.IsOpen = false;
                }
            }

            //enter key pressed, insert the first item to richtextbox
            if ((e.Key == Key.Enter || e.Key == Key.Space || e.Key == Key.Tab))
            {
                PART_IntellisenseListBox.SelectedIndex = 0;
                if (InsertAssistWord())
                {
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Down)
            {
                PART_IntellisenseListBox.Focus();
            }

            base.OnPreviewKeyDown(e);
        }


        protected override void OnTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            base.OnTextInput(e);
            if (IsAssistKeyPressed == false && e.Text.Length == 1)
            {
                PART_IntellisensePopup.IsOpen = true;
                IsAssistKeyPressed = true;
                ContentAssistSource_CollectionView.Refresh();
            }

            if (IsAssistKeyPressed)
            {
                sbLastWords.Append(e.Text);
                ContentAssistSource_CollectionView.Refresh();
            }
        }

        #endregion
    }
}
