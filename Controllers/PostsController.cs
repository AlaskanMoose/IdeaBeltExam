using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BeltExam.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;

namespace BeltExam.Controllers {
  public class PostsController : Controller {
    private BeltExamContext _context;
    public PostsController(BeltExamContext context){
      _context = context;
    }
    [HttpGet]
    [RouteAttribute("dash")]
    public IActionResult Dash(){
      if(!CheckLogin()){
        return RedirectToAction("Index", "Users");
      }
      User CurrentUser = _context.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("CurrUserId"));
      List<Post> AllPosts = _context.Posts.Include(p => p.Likers).ThenInclude(u => u.User).Include(p => p.User).OrderByDescending(post => post.Likers.Count()).ToList();
      ViewBag.Posts = AllPosts;
      ViewBag.CurrentUser = CurrentUser;
      return View("Dash");
    }
    [HttpPost]
    [RouteAttribute("post")]
    public IActionResult AddPost(Post model){
      User CurrentUser = _context.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("CurrUserId"));
      model.UserId = CurrentUser.UserId;
      _context.Add(model);
      _context.SaveChanges();
      return RedirectToAction("Dash");
    }
    [HttpGet]
    [RouteAttribute("deletepost/{id}")]
    public IActionResult DeletePost(int id){
      Post RetrievedPost = _context.Posts.SingleOrDefault(post => post.PostId == id);
      List<Like> RetrievedLikes = _context.Likes.Where(like => like.PostId == id).ToList();
      foreach(var like in RetrievedLikes){
        _context.Likes.Remove(like);
      }
      _context.Posts.Remove(RetrievedPost);
      _context.SaveChanges();
      return RedirectToAction("Dash");
    }
    [HttpGet]
    [RouteAttribute("likepost/{id}")]
    public IActionResult LikePost(int id){
      User CurrentUser = _context.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("CurrUserId"));
      Like NewLike = new Like {
        UserId = CurrentUser.UserId,
        PostId = id,
      };
      _context.Add(NewLike);
      _context.SaveChanges();
      return RedirectToAction("Dash");
    }
    [HttpGet]
    [RouteAttribute("unlike/{id}")]
    public IActionResult UnLikePost(int id){
      User CurrentUser = _context.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("CurrUserId"));
      Like RetrievedLike = _context.Likes.SingleOrDefault(like => like.UserId == CurrentUser.UserId && like.PostId == id);
      _context.Remove(RetrievedLike);
      _context.SaveChanges();
      return RedirectToAction("Dash");
    }
    [HttpGet]
    [RouteAttribute("showuser/{id}")]
    public IActionResult ShowUser(int id){
      ViewBag.User = _context.Users.Include(u => u.Posts).Include(u => u.LikedPosts).SingleOrDefault(user => user.UserId == id);
      return View("ShowUser");
    }
    [HttpGet]
    [RouteAttribute("showpost/{id}")]
    public IActionResult ShowPost(int id){
      Post RetrievedPost = _context.Posts.Include(p => p.Likers).ThenInclude(l => l.User).ToList().SingleOrDefault(post => post.PostId == id);
      ViewBag.Post = RetrievedPost;
      return View("ShowPost");
    }
    private bool CheckLogin(){
        return (HttpContext.Session.GetInt32("CurrUserId") != null);
    }
  }
}