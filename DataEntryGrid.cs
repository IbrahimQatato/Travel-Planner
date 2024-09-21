using Gtk;
using System;

class DataEntryGrid : Gtk.Grid {
    private Calendar dateOfEntryCalendar;
    private Calendar dateOfExitCalendar;
    private TextView messageTextView;

    public DataEntryGrid(int row) {
        Attach(new Label("Date of Entry:"), 0, 0 + row, 1, 1);
        dateOfEntryCalendar = new Calendar();
        Attach(dateOfEntryCalendar, 0, 1 + row, 1, 1);
        
        Attach(new Label("Date of Exit:"), 1, 0 + row, 1, 1);
        dateOfExitCalendar = new Calendar();
        Attach(dateOfExitCalendar, 1, 1 + row, 1, 1);

        Attach(new Label("Message:"), 2, 0 + row, 1, 1);
        messageTextView = new TextView();
        messageTextView.Editable = false;
        messageTextView.Justification = Justification.Center; 
        Attach(messageTextView, 2, 1 + row, 1, 1);
        
        ColumnSpacing = 10;
        RowSpacing = 5;

        dateOfEntryCalendar.DaySelected += OnDateOfEntrySelected!;
        dateOfExitCalendar.DaySelected += OnDateOfExitSelected!;
    }

    private void OnDateOfEntrySelected(object sender, EventArgs e) {
        if (GetDateOfExit() <= GetDateOfEntry()) {
            dateOfExitCalendar.Date = GetDateOfEntry().AddDays(1);
        }
    }

    private void OnDateOfExitSelected(object sender, EventArgs e) {
        if (GetDateOfExit() <= GetDateOfEntry()) {
            dateOfExitCalendar.Date = GetDateOfEntry().AddDays(1);
            SetMessage("Date of Exit must be later than Date of Entry.");
        } else {
            SetMessage("");
        }
    }

    public DateTime GetDateOfEntry() {
        return new DateTime(dateOfEntryCalendar.Date.Year, 
                            dateOfEntryCalendar.Date.Month, 
                            dateOfEntryCalendar.Date.Day);
    }

    public DateTime GetDateOfExit() {
        return new DateTime(dateOfExitCalendar.Date.Year, 
                            dateOfExitCalendar.Date.Month, 
                            dateOfExitCalendar.Date.Day);
    }

    public void SetMessage(string msg) {
        messageTextView.Buffer.Text = msg;
    }
}
