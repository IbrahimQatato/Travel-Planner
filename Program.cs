using System;
using System.Collections.Generic;
using Gdk;
using Gtk;
using static Gtk.Orientation;
using static TestScript;

class MyWindow : Gtk.Window
{
    private List<DataEntryGrid> grids;
    private Box vbox;

    public MyWindow() : base("containers")
    {
        grids = new List<DataEntryGrid>();

        Box top_hbox = new Box(Horizontal, 5);

        Button addButton = new Button("Add");
        addButton.Clicked += OnAddButtonClicked!;
        top_hbox.Add(addButton);

        Button removeButton = new Button("Remove");
        removeButton.Clicked += OnRemoveButtonClicked!;
        top_hbox.Add(removeButton);

        Button calculateButton = new Button("Calculate");
        calculateButton.Clicked += OnCalculateButtonClicked!;
        top_hbox.Add(calculateButton);

        vbox = new Box(Vertical, 5);

        vbox.Add(top_hbox);
        // Add initial grid
        AddNewGrid();

        Add(vbox);
    }
    private void OnRemoveButtonClicked(object sender, EventArgs e)
    {
        if (grids.Count > 1)
        {
            var lastGrid = grids[grids.Count - 1];
            vbox.Remove(lastGrid);
            grids.RemoveAt(grids.Count - 1);
            Console.WriteLine("Remove button clicked");
        }
        else
        {
            Console.WriteLine("No grids to remove");
        }
    }
    private void AddNewGrid()
    {
        DataEntryGrid grid = new DataEntryGrid(grids.Count + 1);
        grids.Add(grid);
        vbox.PackStart(grid, false, false, 0);
        ShowAll();
    }

    private void OnAddButtonClicked(object sender, EventArgs e)
    {
        AddNewGrid();
        Console.WriteLine("Add button clicked");
    }

    public bool checkDates(){
        DateTime previous = grids[0].GetDateOfExit();
        for (int i = 1; i < grids.Count ; ++i) {
            DateTime current = grids[i].GetDateOfEntry();
            if (current < previous) {
                 ShowMessageDialog("Invalid date(s) entered. \n Each date entry must be after the last entry");
                return false;
            }
            previous = current;
        }
        return true;
    }

    private void OnCalculateButtonClicked(object sender, EventArgs e)
    {
        if (!checkDates()){
            return;
        }
        
        List<DateTime> entryDates = new List<DateTime>();
        List<DateTime> exitDates = new List<DateTime>();
        foreach (var grid in grids)
        {
            DateTime? dateOfEntry = grid.GetDateOfEntry();
            DateTime? dateOfExit = grid.GetDateOfExit();

            if (dateOfEntry.HasValue && dateOfExit.HasValue)
            {
                entryDates.Add(dateOfEntry.Value);
                exitDates.Add(dateOfExit.Value);
            }
            else
            {
                ShowMessageDialog("Invalid date(s) entered.\n Correct Input: 'yyyy-MM-dd', 'MM/dd/yyyy', or 'dd-MMM-yyyy'");
                return;
            }
        }
        string message;
        bool flag;
        (message, flag) = TimePeriod.CalculateRemainingDays(entryDates, exitDates);

        if (!flag){
            ShowMessageDialog("Invalid date(s) entered.\n You already exceeded the 90 day period");
            return;
        }

        string[] lines = message.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        
        for (int i = 1; i < lines.Length; ++i) {
            grids[i-1].SetMessage(lines[i]);
        }

        ShowMessageDialog(lines[0]);
    }

    protected override bool OnDeleteEvent(Event e)
    {
        Application.Quit();
        return true;
    }
    public static void PrintDates(List<DateTime> dates)
    {
        foreach (var date in dates)
        {
            Console.WriteLine(date.ToShortDateString());
        }
    }
    private void ShowMessageDialog(string message)
    {
        MessageDialog md = new MessageDialog(
            this,
            DialogFlags.DestroyWithParent,
            MessageType.Info,
            ButtonsType.Ok,
            message
        );

        md.Run();
        md.Destroy();
    }
}

class Hello
{
    static void Main()
    {
        // Console.WriteLine(TestScript.RunScript());
        Application.Init();
        MyWindow w = new MyWindow();
        w.ShowAll();
        Application.Run();
    }
}
