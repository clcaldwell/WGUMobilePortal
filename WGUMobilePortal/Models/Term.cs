﻿using SQLite;

using System;
using System.Collections.Generic;
using System.Linq;

namespace WGUMobilePortal.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string strCourseId
        {
            get
            {
                if ( _courseId == null && !string.IsNullOrEmpty(_strCourseId))
                {
                    return _strCourseId;
                } else if ( _courseId != null )
                {
                    _courseId.Sort();
                    List<string> strList = _courseId.Select(x => x.ToString()).ToList();
                    return string.Join(",", strList);
                } else
                {
                    return null;
                }
            }
            set => _strCourseId = value;
        }

        [SQLite.Ignore]
        public List<int> CourseId
        {
            get
            {
                if (_courseId == null && !string.IsNullOrEmpty(_strCourseId))
                {
                    List<string> strCourseList = _strCourseId.Split(',').ToList();
                    List<int> intList = strCourseList.Select(x => int.Parse(x)).ToList();
                    intList.Sort();
                    _courseId = intList;
                    return _courseId;
                }
                else if (_courseId != null)
                {
                    _courseId.Sort();
                    return _courseId;
                    //List<string> strList = _courseId.Select(x => x.ToString()).ToList();
                    //return string.Join(",", strList);
                }
                else
                {
                    return null;
                }
            }
            set => _courseId = value;
        }

        private List<int> _courseId;
        private string _strCourseId;

        //[SQLite.Ignore]
        //public List<int> intCourseId
        //{

        //}
        //private string _strCourseId;
        //private List<int> _intCourseId;


    }
}
