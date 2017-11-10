using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IMS_Zadanie_2
{
    public partial class MainPage : ContentPage
    {
        HttpClient client;
        private ObservableCollection<Post> _posts;        private const string Url = "http://www.automobilova-mechatronika.fei.stuba.sk/webstranka/?q=node/59/";
        public class Post
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }

        public class Test
        {
            public string Test1 { get; set; }
            public string Test2 { get; set; }
            public string Test3 { get; set; }
            public string Test4 { get; set; }
            public string Test5 { get; set; }
            public string Test6 { get; set; }
        }

        public MainPage()
        {
            InitializeComponent();
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            getText();
        }

        private async Task<string> getText()
        {
            string text = await client.GetStringAsync(Url);
            Int32 indexNodeStart = text.IndexOf("?q=node/") + 12;
            string webStranka = text.Substring(indexNodeStart - 12, 10);
            Int32 indexNodeEnd;

            while (indexNodeStart > 0)
            {
                indexNodeEnd = text.IndexOf("</a>", indexNodeStart);
                string textButton = text.Substring(indexNodeStart, indexNodeEnd - indexNodeStart);
                Button buttonik = new Button();
                buttonik.Text = textButton;//.Substring(0, 11);
                buttonik.StyleId = webStranka;
                buttonik.Clicked += Buttonik_Click;
                stackPanel1.Children.Add(buttonik);

                if (indexNodeStart > text.LastIndexOf("?q=node/"))
                    break;

                indexNodeStart = text.IndexOf("?q=node/", indexNodeStart + 1) + 12;
                webStranka = text.Substring(indexNodeStart - 12, 10);
            }

            return "";
        }
/*
        public async void RefreshDataAsync()
        {
            var response = await client.GetAsync(Url);//GetStringAsync(Url);
         //   var posts = JsonConvert.DeserializeObject<List<Post>>(content);
         //   _posts = new ObservableCollection<Post>(posts);
         //   postsListView.ItemsSource = _posts;

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(content);
                postsListView.ItemsSource = posts;
            }

        }
*/
        private async void Buttonik_Click(object sender, EventArgs e)
        {
            string content = (sender as Button).StyleId.ToString();
            HttpClient webclient1 = new HttpClient();
            string stranka = "http://www.automobilova-mechatronika.fei.stuba.sk/webstranka/" + content;
            string text = await client.GetStringAsync(stranka);
            var fs = new FormattedString();       

            int nadpisZaciatok = text.IndexOf("<h3>");
            int nadpisKoniec = text.IndexOf("</h3>");
            if (text.Substring(nadpisZaciatok, nadpisKoniec - nadpisZaciatok).Contains("<i class="))
            {
                int icko = text.IndexOf("</i>", nadpisZaciatok + 26) + 4;
                string nadpisok = text.Substring(icko, nadpisKoniec - (icko)) + System.Environment.NewLine;
                fs.Spans.Add(new Span { Text = nadpisok, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
            }

            else {
                string nadpisok = text.Substring(nadpisZaciatok, nadpisKoniec - nadpisZaciatok) + System.Environment.NewLine;
                fs.Spans.Add(new Span { Text = nadpisok, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
            }

            int spanPosition;
            int pPosition;
            string odstavce = "";
            int indexZaciatok = text.IndexOf("<p");
            indexZaciatok = indexZaciatok + 3;

            string nadpis;
            while (nadpisZaciatok != -1)
            {
                spanPosition = text.IndexOf("<span", nadpisKoniec + 3) + 19;
                pPosition = text.IndexOf("<p", nadpisKoniec + 3);

                if (spanPosition < pPosition)
                {
                    odstavce = text.Substring(spanPosition + 4, text.IndexOf("</span", nadpisKoniec + 4) - spanPosition - 4);
                    fs.Spans.Add(new Span { Text = odstavce, ForegroundColor = Color.Green, FontSize = 16, FontAttributes = FontAttributes.Italic });

                }

                else if (spanPosition > pPosition)
                {
                    odstavce = text.Substring(pPosition + 3, text.IndexOf("</p>", nadpisKoniec + 4) - pPosition);
                    fs.Spans.Add(new Span { Text = odstavce, ForegroundColor = Color.Black, FontSize = 12, FontAttributes = FontAttributes.Italic });
                }

                nadpisZaciatok = text.IndexOf("<h3>", nadpisKoniec + 1);
                if (nadpisZaciatok < 0)
                    break;

                nadpisKoniec = text.IndexOf("</h3>", nadpisZaciatok);
                if (text.Substring(nadpisZaciatok, nadpisKoniec - (nadpisZaciatok)).Contains("<i class="))
                {
                    int icko = text.IndexOf("</i>", nadpisZaciatok + 26)+4;
                    nadpis = System.Environment.NewLine + System.Environment.NewLine + text.Substring(icko, nadpisKoniec - icko) + System.Environment.NewLine;
                    fs.Spans.Add(new Span { Text = nadpis, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
                }
                else
                {
                    nadpis = System.Environment.NewLine + System.Environment.NewLine + text.Substring(nadpisZaciatok + 4, nadpisKoniec - (nadpisZaciatok+4)) + System.Environment.NewLine;
                    fs.Spans.Add(new Span { Text = nadpis, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
                }
            }
            stackPanel1.IsVisible = false;
            stackPanel2.IsVisible = true;
            labelText.FormattedText = fs;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            labelText.Text = "";
           // string content = (sender as Button).StyleId.ToString();
            HttpClient webclient1 = new HttpClient();
            string stranka = "http://www.automobilova-mechatronika.fei.stuba.sk/webstranka/?q=node/97/";
            string text = await client.GetStringAsync(stranka);
            var fs = new FormattedString();

            int nadpisZaciatok = text.IndexOf("<h3>");
            int nadpisKoniec = text.IndexOf("</h3>");
            if (text.Substring(nadpisZaciatok, nadpisKoniec - nadpisZaciatok).Contains("<i class="))
            {
                int icko = text.IndexOf("</i>", nadpisZaciatok + 26) + 4;
                string nadpisok = text.Substring(icko, nadpisKoniec - (icko)) + System.Environment.NewLine;
                fs.Spans.Add(new Span { Text = nadpisok, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
            }

            else
            {
                string nadpisok = text.Substring(nadpisZaciatok, nadpisKoniec - nadpisZaciatok) + System.Environment.NewLine;
                fs.Spans.Add(new Span { Text = nadpisok, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
            }

            int spanPosition;
            int pPosition;
            string odstavce = "";
            int indexZaciatok = text.IndexOf("<p");
            indexZaciatok = indexZaciatok + 3;

            string nadpis;
            while (nadpisZaciatok != -1)
            {
                spanPosition = text.IndexOf("<span", nadpisKoniec + 3) + 19;
                pPosition = text.IndexOf("<p", nadpisKoniec + 3);

                if (spanPosition < pPosition)
                {
                    odstavce = text.Substring(spanPosition + 4, text.IndexOf("</span", nadpisKoniec + 4) - spanPosition - 4);
                    fs.Spans.Add(new Span { Text = odstavce, ForegroundColor = Color.Green, FontSize = 16, FontAttributes = FontAttributes.Italic });

                }

                else if (spanPosition > pPosition)
                {
                    odstavce = text.Substring(pPosition + 3, text.IndexOf("</p>", nadpisKoniec + 4) - pPosition);
                    fs.Spans.Add(new Span { Text = odstavce, ForegroundColor = Color.Black, FontSize = 12, FontAttributes = FontAttributes.Italic });
                }

                nadpisZaciatok = text.IndexOf("<h3>", nadpisKoniec + 1);
                if (nadpisZaciatok < 0)
                    break;

                nadpisKoniec = text.IndexOf("</h3>", nadpisZaciatok);
                if (text.Substring(nadpisZaciatok, nadpisKoniec - (nadpisZaciatok)).Contains("<i class="))
                {
                    int icko = text.IndexOf("</i>", nadpisZaciatok + 26) + 4;
                    nadpis = System.Environment.NewLine + System.Environment.NewLine + text.Substring(icko, nadpisKoniec - icko) + System.Environment.NewLine;
                    fs.Spans.Add(new Span { Text = nadpis, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
                }
                else
                {
                    nadpis = System.Environment.NewLine + System.Environment.NewLine + text.Substring(nadpisZaciatok + 4, nadpisKoniec - (nadpisZaciatok + 4)) + System.Environment.NewLine;
                    fs.Spans.Add(new Span { Text = nadpis, ForegroundColor = Color.Red, FontSize = 20, FontAttributes = FontAttributes.Italic });
                }
            }
            stackPanel1.IsVisible = false;
            stackPanel2.IsVisible = true;
            tabulka.IsVisible = true;
            labelText.FormattedText = fs;

            //**************************************************************************************
            tabulka.Intent = TableIntent.Settings;
            var layout = new StackLayout() { Orientation = StackOrientation.Horizontal };

            int[] indexTabulka = new int[5];
            indexTabulka[0] = text.IndexOf("tabulka-mvi");
            indexTabulka[1] = text.IndexOf("tabulka-mvi", indexTabulka[0] + 1);
            indexTabulka[2] = text.IndexOf("tabulka-mvi", indexTabulka[1] + 1);
            indexTabulka[3] = text.IndexOf("tabulka-mvi", indexTabulka[2] + 1);
            indexTabulka[4] = 100000000;
            int tdZaciatok, tdKoniec;
            int tdKonecnyKoniec = text.LastIndexOf("</td>");
            string[] textyDoGridu = new string[10];
            int poradoveCisloDoGridu = 0;

            for (int i = 0; i < 4; i++)
            {
                int thPositionStart = text.IndexOf("<th>", indexTabulka[i]);
                int thPositionEnd;
                int trKoniec;

                if (i == 0)
                {
                    var data = new Test { Test1 = "1.Ročník", Test2 = "Zimný semester" };
                    layout.Children.Add(new Label()
                    {
                        Text = "1.Ročník",
                        TextColor = Color.FromHex("#f35e20"),
                        VerticalOptions = LayoutOptions.Center
                    });
                    layout.Children.Add(new Label()
                    {
                        Text = "Zimný semester",
                        TextColor = Color.FromHex("#503026"),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.EndAndExpand
                    });

                    var layout2 = new StackLayout() { Orientation = StackOrientation.Horizontal };
                    layout2.Children.Add(new Label()
                    {
                        Text = "2.Ročník",
                        TextColor = Color.FromHex("#f35e20"),
                        VerticalOptions = LayoutOptions.Center
                    });
                    layout2.Children.Add(new Label()
                    {
                        Text = "Letný semester",
                        TextColor = Color.FromHex("#503026"),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.EndAndExpand
                    });

                    tabulka.Root = new TableRoot() {
                        new TableSection("Getting Started") {
                            new ViewCell() {View = layout},
                            new ViewCell() {View = layout2}
                        }
                    };
                }               
            }
        }
    }
}
