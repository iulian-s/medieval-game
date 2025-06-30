using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
    int speed = 300;
    string current_direction;
    private AnimatedSprite2D animatedSprite;
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
        {
            current_direction = "right";
            play_anim(1);
            direction.X += 1;
        }
        if (Input.IsActionPressed("ui_left"))
        {
            current_direction = "left";
            play_anim(1);
            direction.X -= 1;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            current_direction = "down";
            play_anim(1);
            direction.Y += 1;
        }

        if (Input.IsActionPressed("ui_up"))
        {
            current_direction = "up";
            play_anim(1);
            direction.Y -= 1;
        }

        if (direction != Godot.Vector2.Zero)
        {
            direction = direction.Normalized();

        }
        else
        {
            play_anim(0);
        }

        Velocity = direction * speed;
    }

    public void play_anim(sbyte movement)
    {
        var dir = current_direction;
        var anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (dir == "right")
        {
            anim.FlipH = false;
            if (movement == 1)
            {
                anim.Play("run");
            }
            else if (movement == 0)
            {
                anim.Play("idle");
            }
        }
        if (dir == "left")
        {
            anim.FlipH = true;
            if (movement == 1)
            {
                anim.Play("run");
            }
            else if (movement == 0)
            {
                anim.Play("idle");
            }
        }

        if (dir == "up")
        {
            
            if (movement == 1)
            {
                anim.Play("run_up");
            }
            else if (movement == 0)
            {
                anim.Play("idle");
            }
        }

        if (dir == "down")
        {
            
            if (movement == 1)
            {
                anim.Play("run_down");
            }
            else if (movement == 0)
            {
                anim.Play("idle");
            }
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
            if(animatedSprite.FlipH)
            {
                targetRotation *= -1;
            }
        }
        else targetRotation = 0;
        animatedSprite.RotationDegrees = Mathf.Lerp(animatedSprite.RotationDegrees, targetRotation, delta * 10);
    }
}
