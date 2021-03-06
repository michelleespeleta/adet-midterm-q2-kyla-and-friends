﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationAndAuthorization.Data;
using AuthenticationAndAuthorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAndAuthorization.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public StudentController(AppDbContext db, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var student = await _userManager.FindByIdAsync(id);
            var grade = await _db.StudentGrades.FirstOrDefaultAsync(i => i.AppUserID == id);
            if (grade == null)
            {
                return NotFound();
            }
            grade.Prelim = await _db.GradesPerTerms.FindAsync(grade.PrelimID);
            grade.Prelim.Quiz1 = await _db.ItemScores.FindAsync(grade.Prelim.Quiz1ID);
            grade.Prelim.Quiz2 = await _db.ItemScores.FindAsync(grade.Prelim.Quiz2ID);
            grade.Prelim.Quiz3 = await _db.ItemScores.FindAsync(grade.Prelim.Quiz3ID);
            grade.Prelim.Assignment1 = await _db.ItemScores.FindAsync(grade.Prelim.Assignment1ID);
            grade.Prelim.Assignment2 = await _db.ItemScores.FindAsync(grade.Prelim.Assignment2ID);
            grade.Prelim.Assignment3 = await _db.ItemScores.FindAsync(grade.Prelim.Assignment3ID);
            var pQ1 = grade.Prelim.Quiz1.Grade;
            var pQ2 = grade.Prelim.Quiz2.Grade;
            var pQ3 = grade.Prelim.Quiz3.Grade;
            var pA1 = grade.Prelim.Assignment1.Grade;
            var pA2 = grade.Prelim.Assignment2.Grade;
            var pA3 = grade.Prelim.Assignment3.Grade;

            grade.Prelim.Grade = Math.Round(((pQ1 + pQ2 + pQ3 + pA1 + pA2 + pA3) / 6), 2);

            grade.Midterm = await _db.GradesPerTerms.FindAsync(grade.MidtermID);
            grade.Midterm.Quiz1 = await _db.ItemScores.FindAsync(grade.Midterm.Quiz1ID);
            grade.Midterm.Quiz2 = await _db.ItemScores.FindAsync(grade.Midterm.Quiz2ID);
            grade.Midterm.Quiz3 = await _db.ItemScores.FindAsync(grade.Midterm.Quiz3ID);
            grade.Midterm.Assignment1 = await _db.ItemScores.FindAsync(grade.Midterm.Assignment1ID);
            grade.Midterm.Assignment2 = await _db.ItemScores.FindAsync(grade.Midterm.Assignment2ID);
            grade.Midterm.Assignment3 = await _db.ItemScores.FindAsync(grade.Midterm.Assignment3ID);

            var mQ1 = grade.Midterm.Quiz1.Grade;
            var mQ2 = grade.Midterm.Quiz2.Grade;
            var mQ3 = grade.Midterm.Quiz3.Grade;
            var mA1 = grade.Midterm.Assignment1.Grade;
            var mA2 = grade.Midterm.Assignment2.Grade;
            var mA3 = grade.Midterm.Assignment3.Grade;

            grade.Midterm.Grade = Math.Round(((mQ1 + mQ2 + mQ3 + mA1 + mA2 + mA3) / 6), 2);

            grade.Prefinal = await _db.GradesPerTerms.FindAsync(grade.PrefinalID);
            grade.Prefinal.Quiz1 = await _db.ItemScores.FindAsync(grade.Prefinal.Quiz1ID);
            grade.Prefinal.Quiz2 = await _db.ItemScores.FindAsync(grade.Prefinal.Quiz2ID);
            grade.Prefinal.Quiz3 = await _db.ItemScores.FindAsync(grade.Prefinal.Quiz3ID);
            grade.Prefinal.Assignment1 = await _db.ItemScores.FindAsync(grade.Prefinal.Assignment1ID);
            grade.Prefinal.Assignment2 = await _db.ItemScores.FindAsync(grade.Prefinal.Assignment2ID);
            grade.Prefinal.Assignment3 = await _db.ItemScores.FindAsync(grade.Prefinal.Assignment3ID);

            var rQ1 = grade.Prefinal.Quiz1.Grade;
            var rQ2 = grade.Prefinal.Quiz2.Grade;
            var rQ3 = grade.Prefinal.Quiz3.Grade;
            var rA1 = grade.Prefinal.Assignment1.Grade;
            var rA2 = grade.Prefinal.Assignment2.Grade;
            var rA3 = grade.Prefinal.Assignment3.Grade;

            grade.Prefinal.Grade = Math.Round(((rQ1 + rQ2 + rQ3 + rA1 + rA2 + rA3) / 6), 2);

            grade.Final = await _db.GradesPerTerms.FindAsync(grade.FinalID);
            grade.Final.Quiz1 = await _db.ItemScores.FindAsync(grade.Final.Quiz1ID);
            grade.Final.Quiz2 = await _db.ItemScores.FindAsync(grade.Final.Quiz2ID);
            grade.Final.Quiz3 = await _db.ItemScores.FindAsync(grade.Final.Quiz3ID);
            grade.Final.Assignment1 = await _db.ItemScores.FindAsync(grade.Final.Assignment1ID);
            grade.Final.Assignment2 = await _db.ItemScores.FindAsync(grade.Final.Assignment2ID);
            grade.Final.Assignment3 = await _db.ItemScores.FindAsync(grade.Final.Assignment3ID);

            var fQ1 = grade.Final.Quiz1.Grade;
            var fQ2 = grade.Final.Quiz2.Grade;
            var fQ3 = grade.Final.Quiz3.Grade;
            var fA1 = grade.Final.Assignment1.Grade;
            var fA2 = grade.Final.Assignment2.Grade;
            var fA3 = grade.Final.Assignment3.Grade;

            grade.Final.Grade = Math.Round(((fQ1 + fQ2 + fQ3 + fA1 + fA2 + fA3) / 6), 2);

            var pGrade = grade.Prelim.Grade;
            var mGrade = grade.Midterm.Grade;
            var rGrade = grade.Prefinal.Grade;
            var fGrade = grade.Final.Grade;

            grade.SubjectGrade = Math.Round(((pGrade + mGrade + rGrade + fGrade) / 4), 2);
            return View(grade);
            
        }
    }
}
