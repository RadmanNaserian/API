using System;
using System.ComponentModel;

public class StudentModel
{
    [DefaultValue(0)]
    public int Id { get; set; } = 0;

    [DefaultValue("")]
    public string FirstName { get; set; } = "";

    [DefaultValue("")]
    public string LastName { get; set; } = "";
}
