using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models{
  public class User : BaseEntity{
    [Key]
    public int UserId {get; set; }
    [Required]
    public string FirstName {get; set; }
    [Required]
    public string Alias {get; set; }
    [Required]
    [EmailAddress]
    public string Email {get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password {get; set; }
    [Compare("Password", ErrorMessage="Passwords must match")]
    [DataType(DataType.Password)]
    public string PasswordConfirmation {get; set; }
    public List<Post> Posts{ get; set; }
    public List<Like> LikedPosts {get; set;}
    public User(){
      Posts = new List<Post>();
      LikedPosts = new List<Like>();
    }  
  }
}