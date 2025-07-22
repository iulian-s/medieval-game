using Godot;
using System;

public partial class Undead : CharacterBody2D
{
    CollisionShape2D collisionShape;
    Player player;
    AnimatedSprite2D animatedSprite;
    int speed = 30;
    int health = 100;
    int damage;
    bool isDead = false;
    bool player_in_area = false;
    bool canAttack = false;
    float attackCooldown = 1.0f;
    float attackTimer = 0.0f;

    bool isAttacking = false;


    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>("detection/CollisionShape2D");
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        isDead = false;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!isDead)
        {
            collisionShape.Disabled = false;
            if (player_in_area)
            {
                Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
                Velocity = direction * speed;
                
                animatedSprite.Play("move");
                if (canAttack && !isAttacking)
                {
                    Velocity = Vector2.Zero;
                    PerformAttack();
                }
                
                if (direction.X < 0)
                {
                    animatedSprite.FlipH = true;
                }
                else if (direction.X > 0)
                {
                    animatedSprite.FlipH = false;
                }
            }
            else
            {
                Velocity = Vector2.Zero;
                animatedSprite.Play("idle");
            }
            MoveAndSlide();
        }
        else
        {
            Velocity = Vector2.Zero;
            collisionShape.Disabled = true;
            animatedSprite.Play("death");
            if (!animatedSprite.IsPlaying())
            {
                QueueFree();
            }
        }

    }

    public void _on_detection_body_entered(Node body)
    {
        if (body.HasMethod("player"))
        {
            player = (Player)body;
            player_in_area = true;
        }
    }
    public void _on_detection_body_exited(Node body)
    {
        if (body.HasMethod("player"))
        {
            player_in_area = false;
        }
    }

    public void _on_attack_area_body_entered(Node body)
    {
        if (body.HasMethod("player"))
        {
            player = (Player)body;
            canAttack = true;
        }
    }
    public void _on_attack_area_body_exited(Node body)
    {
        if (body is Player)
        {
            canAttack = false;
        }
    }


    public async void PerformAttack()
    {
        if (isAttacking) return;

    isAttacking = true;
    GD.Print("Undead attacks player");

    animatedSprite.Play("attack");
    
    await ToSignal(GetTree().CreateTimer(attackCooldown), "timeout"); // cooldown wait

    isAttacking = false;
        
    }
    public void enemy(){}
}
