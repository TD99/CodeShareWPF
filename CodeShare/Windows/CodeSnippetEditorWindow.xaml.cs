﻿using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CodeShare.MVVM.Model;
using System.Windows.Controls;

namespace CodeShare.Windows
{
    public partial class CodeSnippetEditorWindow : Window
    {
        private Control? _initiator;
        private string? _code;
        private string? _name;
        private string? _extension;
        private Language? _language;
        private List<Language>? _languages;

        private bool _isFocusFlop = false;
        private bool _isFullyLoaded = false;

        public CodeSnippetEditorWindow(string? code = "", string? name = "", string? extension = "", Language? language = null, Control? initiator = null)
        {
            InitializeComponent();
            _initiator = initiator;
            _code = code;
            _name = name;
            _extension = extension;
            _language = language;

            WebView2Control.CoreWebView2InitializationCompleted += WebView2Control_OnCoreWebView2InitializationCompleted;
            WebView2Control.PreviewKeyDown += (sender, e) =>
            {
                e.Handled = e.Key switch
                {
                    Key.F5 => true,
                    _ => e.Handled
                };
            };
        }

        private void WebView2Control_OnLoaded(object sender, RoutedEventArgs e)
        {
            WebView2Control.Source = new Uri(System.IO.Path.GetFullPath("Plugins/Monaco/index.html"));
        }

        private void WebView2Control_OnCoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                WebView2Control.CoreWebView2.NavigationCompleted += CoreWebView2_OnNavigationCompleted;
            }
            else
            {
                MessageBox.Show("WebView2 could not be initialized. Check app permissions.");
            }
        }

        private async void CoreWebView2_OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                _isFullyLoaded = true;
                await SetCurrentLanguages();
                FocusDefault();
            }
            else
            {
                MessageBox.Show("The internal code editor could not be loaded.");
            }
        }

        private void FocusDefault()
        {
            if (InputTitle.Text.Length < 1)
            {
                InputTitle.TextBox.Focus();
            }
            else
            {
                WebView2Control.Focus();
            }
        }

        private async Task OpenCode(string? code, string? extension)
        {
            var result = await WebView2Control.CoreWebView2.ExecuteScriptAsync($"openText({JsonConvert.SerializeObject(code)}, {JsonConvert.SerializeObject(extension)});");
            var stringResult = _language?.Id ?? JsonConvert.DeserializeObject<string>(result) ?? "plaintext";
            InputLanguage.ComboBox.SelectedValue = stringResult;
            _language = _languages?.Find(l => l.Id == stringResult);
            InputTitle.Text = _name ?? "";
        }

        private async Task<string> GetCode()
        {
            var result = await WebView2Control.CoreWebView2.ExecuteScriptAsync("editor.getValue();");
            return JsonConvert.DeserializeObject<string>(result) ?? "";
        }

        private void WebView2Control_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (_isFocusFlop)
            {
                _isFocusFlop = false;
                return;
            }

            _isFocusFlop = true;
            DummyFocus.Focus();
            WebView2Control.Focus();
            WebView2Control.CoreWebView2.ExecuteScriptAsync("editor.focus();");
        }

        private async Task SetCurrentLanguages()
        {
            var result = await WebView2Control.CoreWebView2.ExecuteScriptAsync($"monaco.languages.getLanguages();");
            _languages = JsonConvert.DeserializeObject<List<Language>>(result);

            LoadLanguages(_languages);
            await OpenCode(_code, _extension);
        }

        private void SetEditorLanguage(Language language)
        {
            WebView2Control.CoreWebView2.ExecuteScriptAsync($"setLang({JsonConvert.SerializeObject(language.Id)});");
        }

        private void LoadLanguages(List<Language> languages)
        {
            InputLanguage.ComboBoxItems = languages;
            InputLanguage.ComboBox.SelectedValue = "plaintext";
        }

        private void InputLanguage_OnComboBoxSelectionChangedEvenHandler(object sender, SelectionChangedEventArgs e)
        {
            var lang = InputLanguage.ComboBoxSelectedItem as Language;
            SetEditorLanguage(lang);
        }

        public async void CodeSnippetEditorWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            if (!_isFullyLoaded || _initiator is not ToolbarWindow) return;

            var code = await GetCode();
            if (code.Length <= 0) return;

            App.OpenToolbarWindow(new ToolbarWindow(code, InputTitle.Text, _extension, InputLanguage.ComboBoxSelectedItem as Language));
        }
    }
}
