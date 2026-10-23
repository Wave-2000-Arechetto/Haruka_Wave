using Godot;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Haruka.Scripts.UI
{
    public partial class EmotionBar : Control
    {
        private Player _player = null!;
        
        [Export] public float BarWidth = 240f;
        [Export] public float BarHeight = 20f;

        private float _displayValue = 0f;
        private float _targetValue = 0f;

        private readonly Color _pinkZero = new Color(1f, 0.92f, 0.94f, 0.6f);
        private readonly Color _pinkFull = new Color(0.93f, 0.38f, 0.52f);
        private readonly Color _purpleZero = new Color(0.92f, 0.92f, 1f, 0.6f);
        private readonly Color _purpleFull = new Color(0.32f, 0.14f, 0.48f);
        private readonly Color _trackBg = new Color(0.08f, 0.08f, 0.12f, 0.85f);

        private readonly Color _trackBorder = new Color(1f, 1f, 1f, 0.6f);
        // ========== 工厂 ==========
        public static EmotionBar Create(Player player)
{
    var bar = new EmotionBar 
    { 
        _player = player,
        BarWidth = 300f,   // 稍短
        BarHeight = 26f    // 更粗更圆润
    };
    bar.CustomMinimumSize = new Vector2I(280, 80); // 给大数字留空间
    bar.Size = bar.CustomMinimumSize;
    return bar;
}

        // ========== 挂载（类似 Sankta） ==========
        public void AttachToPlayer()
        {
            var creatureNode = _player.Creature.GetCreatureNode();
            if (creatureNode == null)
            {
                // Fallback：挂到全局，用固定坐标
                Position = new Vector2(325, 325);
                return;
            }

            // 尝试找到 Bounds（血条/角色框）
            var bounds = creatureNode.GetNodeOrNull<Control>("%Bounds");
            if (bounds != null && bounds.Size.X > 0)
            {
                bounds.AddChild(this);
                // 贴在 Bounds 正上方（头顶）
                Position = new Vector2(bounds.Size.X / 2f - Size.X / 2f, -300f);
            }
            else
            {
                // 没 Bounds 就直接挂 creatureNode，手动调位置
                creatureNode.AddChild(this);
                Position = new Vector2(-Size.X / 2f, -500);
            }
        }

        public override void _Ready()
        {
            _targetValue = 0f;
            _displayValue = 0f;

            if (EmotionBarManager.Instance != null)
            {
                EmotionBarManager.Instance.OnValueChanged += HandleValueChanged;
            }
        }

        private void HandleValueChanged(Player player, float value)
        {
            if (player != _player) return;
            SetValue(value);
        }

        public override void _ExitTree()
        {
            if (EmotionBarManager.Instance != null)
            {
                EmotionBarManager.Instance.OnValueChanged -= HandleValueChanged;
            }
        }

        // ========== 对外刷新接口（类似 Sankta 的 Refresh） ==========
        public void Refresh()
        {
            QueueRedraw();
        }

        public override void _Process(double delta)
        {
            if (!Mathf.IsEqualApprox(_displayValue, _targetValue))
            {
                _displayValue = Mathf.MoveToward(_displayValue, _targetValue, (float)delta * 6f);
                QueueRedraw();
            }
        }

        // ========== 绘制（完全不变） ==========
        public override void _Draw()
{
    float centerX = Size.X / 2f;
    float centerY = Size.Y / 2f;
    float left = centerX - BarWidth / 2f;
    float barTop = centerY - BarHeight / 2f;
    int capRadius = Mathf.CeilToInt(BarHeight / 2f); // 胶囊半径 = 高度一半，完全圆润

    // 1. 外圈柔光/阴影（让条有"泡泡"感）
    var shadow = new StyleBoxFlat();
    shadow.BgColor = new Color(0, 0, 0, 0.15f);
    SetRadius(shadow, capRadius + 8);
    DrawStyleBox(shadow, new Rect2(left - 10, barTop - 10, BarWidth + 20, BarHeight + 20));

    // 2. 背景轨道（完全胶囊形 + 白色描边）
    var track = new StyleBoxFlat();
    track
.BgColor = _trackBg;
    track
.BorderColor = _trackBorder;
    track
.BorderWidthLeft = track.BorderWidthRight = track.BorderWidthTop = track.BorderWidthBottom = 2;
    SetRadius(track, capRadius);
    DrawStyleBox(track, new Rect2(left, barTop, BarWidth, BarHeight));
    // 3. 情绪填充（从中心向两边生长的胶囊段）
    if (Mathf.Abs(_displayValue) > 0.02f)
    {
        bool isPos = _displayValue > 0;
        float valX = ValueToPixel(_displayValue);
        float startX = isPos ? centerX : valX;
        float width = Mathf.Abs(valX - centerX);
        width = Mathf.Max(width, capRadius * 2f); // 再短也是个圆点

        var fill = new StyleBoxFlat();
        fill.BgColor = GetFillColor(_displayValue);

        if (width > capRadius * 2.5f)
        {
            // 足够长：一端圆一端平，衔接成胶囊段
            if (isPos)
            {
                fill.CornerRadiusTopLeft = 0;
                fill.CornerRadiusBottomLeft = 0;
                fill.CornerRadiusTopRight = capRadius;
                fill.CornerRadiusBottomRight = capRadius;
            }
            else
            {
                fill.CornerRadiusTopLeft = capRadius;
                fill.CornerRadiusBottomLeft = capRadius;
                fill.CornerRadiusTopRight = 0;
                fill.CornerRadiusBottomRight = 0;
            }
            DrawStyleBox(fill, new Rect2(startX + 2, barTop + 3, width - 4, BarHeight - 6));
        }
        else
        {
            // 很短时直接画圆点，避免 StyleBox 变形
            float dotX = isPos ? startX + width / 2f : startX - width / 2f;
            DrawCircle(new Vector2(dotX, centerY), capRadius - 3f, fill.BgColor);
        }

        // 填充上的高光细线（增加圆润/光泽感）
        float glossY = barTop + 5f;
        float glossStart = isPos ? centerX + 6f : startX + 6f;
        float glossEnd = isPos ? startX + width - 6f : centerX - 6f;
        if (glossEnd > glossStart)
        {
            DrawLine(
                new Vector2(glossStart, glossY),
                new Vector2(glossEnd, glossY),
                new Color(1, 1, 1, 0.3f), 2f
            );
        }
    }

    // 4. 大数字（条下方居中，字号 24，带阴影）
    string txt = (_displayValue > 0 ? "+" : "") + $"{_displayValue:F1}";
    Color txtColor = Mathf.Abs(_displayValue) < 0.1f
        ? new Color(0.95f, 0.95f, 0.95f)
        : GetFillColor(_displayValue);

    float textY = barTop - 28f;

    // 阴影层
    DrawString(ThemeDB.FallbackFont, new Vector2(centerX + 1, textY + 1),
        txt, HorizontalAlignment.Center, -1, 24, new Color(0, 0, 0, 0.35f));
    // 主文字
    DrawString(ThemeDB.FallbackFont, new Vector2(centerX-25f, textY),
        txt, HorizontalAlignment.Center, -1, 24, txtColor);
}

        private Color GetFillColor(float value)
        {
            float t = Mathf.Abs(value) / 3f;
            t = Mathf.Clamp(t, 0f, 1f);
            if (value >= 0)
                return _pinkZero.Lerp(_pinkFull, t);
            else
                return _purpleZero.Lerp(_purpleFull, t);
        }

        private float ValueToPixel(float value)
        {
            float t = (value + 3f) / 6f;
            float left = Size.X / 2f - BarWidth / 2f;
            return left + t * BarWidth;
        }

        public void SetValue(float v)
        {
            _targetValue = Mathf.Clamp(v, -3f, 3f);
            QueueRedraw();
        }

        private void SetRadius(StyleBoxFlat s, int r)
        {
            s.CornerRadiusTopLeft = r;
            s.CornerRadiusTopRight = r;
            s.CornerRadiusBottomLeft = r;
            s.CornerRadiusBottomRight = r;
        }
    }


    
    
}