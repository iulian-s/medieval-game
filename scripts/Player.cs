using Godot;
using System;
using System.Numerics;

public partial class Player : CharacterBody2D
{
    bool enemy_in_area = false;
    bool enemy_attack_cooldown = true;
    bool player_alive = true;
    int speed = 200;
    string current_direction;
    private AnimatedSprite2D animatedSprite;
    private bool facingLeft = false;
    Undead enemy;
    [Export] private Timer attackCooldown;
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        attackCooldown.Timeout += _on_attack_cooldown_timeout;
        animatedSprite.Play("idle");
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!player_alive)
            return;
        UpdateAnimationRotation((float)delta);
        player_movement();
        enemy_attack();
        if (Global.playerHP <= 0)
        {
            player_alive = false;
            animatedSprite.Play("death");
            GD.Print("Player is dead");
            
        }
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

    public void player() { }

    public void _on_player_hitbox_body_entered(Node body)
    {
        if (body.HasMethod("enemy"))
        {
            enemy_in_area = true;

        }
    }
    public void _on_player_hitbox_body_exited(Node body)
    {
        if (body.HasMethod("enemy"))
        {
            enemy_in_area = false;
        }
    }

    public void enemy_attack()
    {
        if (enemy_in_area && enemy_attack_cooldown)
        {
            Global.playerHP -= 10;
            enemy_attack_cooldown = false;
            attackCooldown.Start();
            GD.Print("Player HP: " + Global.playerHP);
        }
    }

    public void _on_attack_cooldown_timeout()
    {
        enemy_attack_cooldown = true;
    }
}
