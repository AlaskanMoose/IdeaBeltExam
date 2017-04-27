using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models{
    public class Post : BaseEntity{
        public int PostId { get; set; }
        public string Idea {get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Like> Likers { get; set; }
        public Post(){
            Likers = new List<Like>();
        }
    }
}