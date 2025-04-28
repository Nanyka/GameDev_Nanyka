using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<Course> myCourses = new List<Course>();
        School mySchool = new School();
        Student myStudent = new Student();
        
        mySchool.InsertScore(3,myCourses);
        mySchool.InsertScore(8,myCourses);

        myStudent.CheckPass(myCourses);
    }
}

public class CompletedCourse
{
    protected int _score;
    protected bool _isCompleted;

    public CompletedCourse(int score)
    {
        _score = score;
    }

    public void CheckCompleted()
    {
        Debug.Log($"{_score} is completed: {_isCompleted}");
    }
}

public class Course: CompletedCourse
{
    public Course(int score) : base(score)
    {
        _isCompleted = _score >= 5;
    }
}

public class School
{
    public void InsertScore(int score, List<Course> scores)
    {
        scores.Add(new Course(score));
    }
}

public class Student
{
    public void CheckPass(List<Course> courses)
    {
        foreach (var course in courses)
        {
            var completedCourse = course as CompletedCourse;
            completedCourse.CheckCompleted();
        }
    }
}
