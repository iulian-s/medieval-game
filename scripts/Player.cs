using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
    int speed = 300;
    string current_direction;
    private AnimatedSprite2D animatedSprite;
    private bool facingLeft = false;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animatedSprite.Play("idle");
    }
    public override void _PhysicsProcess(double delta)
    {
        UpdateAnimationRotation((float)delta);
        player_movement();
        MoveAndSlide();
    }
    public void player_movement()
    {
        Godot.Vector2 direction = Godot.Vector2.Zero;

        if (Input.IsActionPressed("ui_right"))
        direction.X += 1;
    if (Input.IsActionPressed("ui_left"))
        direction.X -= 1;
    if (Input.IsActionPressed("ui_down"))
        direction.Y += 1;
    if (Input.IsActionPressed("ui_up"))
        direction.Y -= 1;

    if (direction != Godot.Vector2.Zero)
    {
        direction = direction.Normalized();
        current_direction = GetDirectionFromVector(direction);
        play_anim(1);
    }
    else
    {
        play_anim(0);
    }

    Velocity = direction * speed;
    }

    public void play_anim(sbyte movement)
    {
        var anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        switch (current_direction)
        {
            case "right":
                facingLeft = false;
                anim.FlipH = false;
                anim.Play(movement == 1 ? "run" : "idle");
                break;

            case "left":
                facingLeft = true;
                anim.FlipH = true;
                anim.Play(movement == 1 ? "run" : "idle");
                break;

            case "up":
                anim.FlipH = facingLeft;
                anim.Play(movement == 1 ? "run_up" : "idle");
                break;

            case "down":
                anim.FlipH = facingLeft;
                anim.Play(movement == 1 ? "run_down" : "idle");
                break;

            case "up_right":
                anim.FlipH = false;
                anim.Play(movement == 1 ? "run_up" : "idle");
                break;

            case "up_left":
                anim.FlipH = true;
                anim.Play(movement == 1 ? "run_up" : "idle");
                break;

            case "down_right":
                anim.FlipH = false;
                anim.Play(movement == 1 ? "run_down" : "idle");
                break;

            case "down_left":
                anim.FlipH = true;
                anim.Play(movement == 1 ? "run_down" : "idle");
                break;

            default:
                anim.Play("idle");
                break;
        }
    }


    public void UpdateAnimationRotation(float delta)
    {
        float targetRotation = 0;
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (animatedSprite.Animation == "run_up")
        {
            targetRotation = -10;
            if (animatedSprite.FlipH)
            {
                targetRotation *= -1;
            }
        }
        else if (animatedSprite.Animation == "run_down")
        {
            targetRotation = 10;
            if (animatedSprite.FlipH)
            {
                targetRotation *= -1;
            }
        }
        else targetRotation = 0;
        animatedSprite.RotationDegrees = Mathf.Lerp(animatedSprite.RotationDegrees, targetRotation, delta * 10);
    }
    
    private string GetDirectionFromVector(Godot.Vector2 dir)
{
    if (dir.X > 0 && dir.Y < 0) return "up_right";
    if (dir.X < 0 && dir.Y < 0) return "up_left";
    if (dir.X > 0 && dir.Y > 0) return "down_right";
    if (dir.X < 0 && dir.Y > 0) return "down_left";
    if (dir.X > 0) return "right";
    if (dir.X < 0) return "left";
    if (dir.Y < 0) return "up";
    if (dir.Y > 0) return "down";
    return "idle";
}


}
