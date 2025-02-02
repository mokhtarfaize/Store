// Ion.RangeSlider | version 2.0.9 | https://github.com/IonDen/ion.rangeSlider
;
(function (g, q, h, r, u) {
    var t = 0,
    n = function () {
        var a = r.userAgent,
        b = /msie\s\d+/i;
        return 0 < a.search(b) && (a = b.exec(a).toString(), a = a.split(' ')[1], 9 > a) ? (g('html').addClass('lt-ie9'), !0) : !1
    }();
    Function.prototype.bind || (Function.prototype.bind = function (a) {
        var b = this,
        c = [
        ].slice;
        if ('function' != typeof b) throw new TypeError;
        var d = c.call(arguments, 1),
        e = function () {
            if (this instanceof e) {
                var f = function () {
                };
                f.prototype = b.prototype;
                var f = new f,
                k = b.apply(f, d.concat(c.call(arguments)));
                return Object(k) === k ? k : f
            }
            return b.apply(a, d.concat(c.call(arguments)))
        };
        return e
    });
    Array.prototype.indexOf || (Array.prototype.indexOf = function (a, b) {
        var c;
        if (null == this) throw new TypeError('"this" is null or not defined');
        var d = Object(this),
        e = d.length >>> 0;
        if (0 === e) return -1;
        c = +b || 0;
        Infinity === Math.abs(c) && (c = 0);
        if (c >= e) return -1;
        for (c = Math.max(0 <= c ? c : e - Math.abs(c), 0) ; c < e;) {
            if (c in d && d[c] === a) return c;
            c++
        }
        return -1
    });
    var p = function (a, b, c) {
        this.VERSION = '2.0.8';
        this.input = a;
        this.plugin_count = c;
        this.old_to = this.old_from = this.update_tm = this.calc_count =
        this.current_plugin = 0;
        this.raf_id = null;
        this.is_update = this.is_key = this.force_redraw = this.dragging = !1;
        this.is_start = !0;
        this.is_click = this.is_resize = this.is_active = !1;
        this.$cache = {
            win: g(h),
            body: g(q.body),
            input: g(a),
            cont: null,
            rs: null,
            min: null,
            max: null,
            from: null,
            to: null,
            single: null,
            bar: null,
            line: null,
            s_single: null,
            s_from: null,
            s_to: null,
            shad_single: null,
            shad_from: null,
            shad_to: null,
            grid: null,
            grid_labels: [
            ]
        };
        c = this.$cache.input;
        a = {
            type: c.data('type'),
            min: c.data('min'),
            max: c.data('max'),
            from: c.data('from'),
            to: c.data('to'),
            step: c.data('step'),
            min_interval: c.data('minInterval'),
            max_interval: c.data('maxInterval'),
            drag_interval: c.data('dragInterval'),
            values: c.data('values'),
            from_fixed: c.data('fromFixed'),
            from_min: c.data('fromMin'),
            from_max: c.data('fromMax'),
            from_shadow: c.data('fromShadow'),
            to_fixed: c.data('toFixed'),
            to_min: c.data('toMin'),
            to_max: c.data('toMax'),
            to_shadow: c.data('toShadow'),
            prettify_enabled: c.data('prettifyEnabled'),
            prettify_separator: c.data('prettifySeparator'),
            force_edges: c.data('forceEdges'),
            keyboard: c.data('keyboard'),
            keyboard_step: c.data('keyboardStep'),
            grid: c.data('grid'),
            grid_margin: c.data('gridMargin'),
            grid_num: c.data('gridNum'),
            grid_snap: c.data('gridSnap'),
            hide_min_max: c.data('hideMinMax'),
            hide_from_to: c.data('hideFromTo'),
            prefix: c.data('prefix'),
            postfix: c.data('postfix'),
            max_postfix: c.data('maxPostfix'),
            decorate_both: c.data('decorateBoth'),
            values_separator: c.data('valuesSeparator'),
            disable: c.data('disable')
        };
        a.values = a.values && a.values.split(',');
        if (c = c.prop('value')) c = c.split(';'),
        c[0] && c[0] == +c[0] && (c[0] = +c[0]),
        c[1] && c[1] == +c[1] && (c[1] = +c[1]),
        b && b.values && b.values.length ? (a.from = c[0] && b.values.indexOf(c[0]), a.to = c[1] && b.values.indexOf(c[1])) : (a.from = c[0] && +c[0], a.to = c[1] && +c[1]);
        b = g.extend(a, b);
        this.options = g.extend({
            type: 'single',
            min: 10,
            max: 100,
            from: null,
            to: null,
            step: 1,
            min_interval: 0,
            max_interval: 0,
            drag_interval: !1,
            values: [
            ],
            p_values: [
            ],
            from_fixed: !1,
            from_min: null,
            from_max: null,
            from_shadow: !1,
            to_fixed: !1,
            to_min: null,
            to_max: null,
            to_shadow: !1,
            prettify_enabled: !0,
            prettify_separator: ' ',
            prettify: null,
            force_edges: !1,
            keyboard: !1,
            keyboard_step: 5,
            grid: !1,
            grid_margin: !0,
            grid_num: 4,
            grid_snap: !1,
            hide_min_max: !1,
            hide_from_to: !1,
            prefix: '',
            postfix: '',
            max_postfix: '',
            decorate_both: !0,
            values_separator: ' — ',
            disable: !1,
            onStart: null,
            onChange: null,
            onFinish: null,
            onUpdate: null
        }, b);
        this.validate();
        this.result = {
            input: this.$cache.input,
            slider: null,
            min: this.options.min,
            max: this.options.max,
            from: this.options.from,
            from_percent: 0,
            from_value: null,
            to: this.options.to,
            to_percent: 0,
            to_value: null
        };
        this.coords =
        {
            x_gap: 0,
            x_pointer: 0,
            w_rs: 0,
            w_rs_old: 0,
            w_handle: 0,
            p_gap: 0,
            p_gap_left: 0,
            p_gap_right: 0,
            p_step: 0,
            p_pointer: 0,
            p_handle: 0,
            p_single: 0,
            p_single_real: 0,
            p_from: 0,
            p_from_real: 0,
            p_to: 0,
            p_to_real: 0,
            p_bar_x: 0,
            p_bar_w: 0,
            grid_gap: 0,
            big_num: 0,
            big: [
            ],
            big_w: [
            ],
            big_p: [
            ],
            big_x: [
            ]
        };
        this.labels = {
            w_min: 0,
            w_max: 0,
            w_from: 0,
            w_to: 0,
            w_single: 0,
            p_min: 0,
            p_max: 0,
            p_from: 0,
            p_from_left: 0,
            p_to: 0,
            p_to_left: 0,
            p_single: 0,
            p_single_left: 0
        };
        this.init()
    };
    p.prototype = {
        init: function (a) {
            this.coords.p_step = this.options.step / ((this.options.max -
            this.options.min) / 100);
            this.target = 'base';
            this.toggleInput();
            this.append();
            this.setMinMax();
            if (a) {
                if (this.force_redraw = !0, this.calc(!0), this.options.onUpdate && 'function' === typeof this.options.onUpdate) this.options.onUpdate(this.result)
            } else if (this.force_redraw = !0, this.calc(!0), this.options.onStart && 'function' === typeof this.options.onStart) this.options.onStart(this.result);
            this.updateScene()
        },
        append: function () {
            this.$cache.input.before('<span class="irs js-irs-' + this.plugin_count + '"></span>');
            this.$cache.input.prop('readonly', !0);
            this.$cache.cont = this.$cache.input.prev();
            this.result.slider = this.$cache.cont;
            this.$cache.cont.html('<span class="irs"><span class="irs-line" tabindex="-1"><span class="irs-line-left"></span><span class="irs-line-mid"></span><span class="irs-line-right"></span></span><span class="irs-min">0</span><span class="irs-max">1</span><span class="irs-from">0</span><span class="irs-to">0</span><span class="irs-single">0</span></span><span class="irs-grid"></span><span class="irs-bar"></span>');
            this.$cache.rs =
            this.$cache.cont.find('.irs');
            this.$cache.min = this.$cache.cont.find('.irs-min');
            this.$cache.max = this.$cache.cont.find('.irs-max');
            this.$cache.from = this.$cache.cont.find('.irs-from');
            this.$cache.to = this.$cache.cont.find('.irs-to');
            this.$cache.single = this.$cache.cont.find('.irs-single');
            this.$cache.bar = this.$cache.cont.find('.irs-bar');
            this.$cache.line = this.$cache.cont.find('.irs-line');
            this.$cache.grid = this.$cache.cont.find('.irs-grid');
            'single' === this.options.type ? (this.$cache.cont.append('<span class="irs-bar-edge"></span><span class="irs-shadow shadow-single"></span><span class="irs-slider single"></span>'), this.$cache.s_single = this.$cache.cont.find('.single'), this.$cache.from[0].style.visibility = 'hidden', this.$cache.to[0].style.visibility = 'hidden', this.$cache.shad_single = this.$cache.cont.find('.shadow-single')) : (this.$cache.cont.append('<span class="irs-shadow shadow-from"></span><span class="irs-shadow shadow-to"></span><span class="irs-slider from"></span><span class="irs-slider to"></span>'), this.$cache.s_from = this.$cache.cont.find('.from'), this.$cache.s_to = this.$cache.cont.find('.to'), this.$cache.shad_from =
            this.$cache.cont.find('.shadow-from'), this.$cache.shad_to = this.$cache.cont.find('.shadow-to'), this.setTopHandler());
            this.options.hide_from_to && (this.$cache.from[0].style.display = 'none', this.$cache.to[0].style.display = 'none', this.$cache.single[0].style.display = 'none');
            this.appendGrid();
            this.options.disable ? (this.appendDisableMask(), this.$cache.input[0].disabled = !0) : (this.$cache.cont.removeClass('irs-disabled'), this.$cache.input[0].disabled = !1, this.bindEvents())
        },
        setTopHandler: function () {
            var a = this.options.max,
            b = this.options.to;
            this.options.from > this.options.min && b === a ? this.$cache.s_from.addClass('type_last') : b < a && this.$cache.s_to.addClass('type_last')
        },
        appendDisableMask: function () {
            this.$cache.cont.append('<span class="irs-disable-mask"></span>');
            this.$cache.cont.addClass('irs-disabled')
        },
        remove: function () {
            this.$cache.cont.remove();
            this.$cache.cont = null;
            this.$cache.line.off('keydown.irs_' + this.plugin_count);
            this.$cache.body.off('touchmove.irs_' + this.plugin_count);
            this.$cache.body.off('mousemove.irs_' + this.plugin_count);
            this.$cache.win.off('touchend.irs_' + this.plugin_count);
            this.$cache.win.off('mouseup.irs_' + this.plugin_count);
            n && (this.$cache.body.off('mouseup.irs_' + this.plugin_count), this.$cache.body.off('mouseleave.irs_' + this.plugin_count));
            this.$cache.grid_labels = [
            ];
            this.coords.big = [
            ];
            this.coords.big_w = [
            ];
            this.coords.big_p = [
            ];
            this.coords.big_x = [
            ];
            cancelAnimationFrame(this.raf_id)
        },
        bindEvents: function () {
            this.$cache.body.on('touchmove.irs_' + this.plugin_count, this.pointerMove.bind(this));
            this.$cache.body.on('mousemove.irs_' +
            this.plugin_count, this.pointerMove.bind(this));
            this.$cache.win.on('touchend.irs_' + this.plugin_count, this.pointerUp.bind(this));
            this.$cache.win.on('mouseup.irs_' + this.plugin_count, this.pointerUp.bind(this));
            this.$cache.line.on('touchstart.irs_' + this.plugin_count, this.pointerClick.bind(this, 'click'));
            this.$cache.line.on('mousedown.irs_' + this.plugin_count, this.pointerClick.bind(this, 'click'));
            this.options.drag_interval && 'double' === this.options.type ? (this.$cache.bar.on('touchstart.irs_' + this.plugin_count, this.pointerDown.bind(this, 'both')), this.$cache.bar.on('mousedown.irs_' + this.plugin_count, this.pointerDown.bind(this, 'both'))) : (this.$cache.bar.on('touchstart.irs_' + this.plugin_count, this.pointerClick.bind(this, 'click')), this.$cache.bar.on('mousedown.irs_' + this.plugin_count, this.pointerClick.bind(this, 'click')));
            'single' === this.options.type ? (this.$cache.s_single.on('touchstart.irs_' + this.plugin_count, this.pointerDown.bind(this, 'single')), this.$cache.shad_single.on('touchstart.irs_' + this.plugin_count, this.pointerClick.bind(this, 'click')), this.$cache.s_single.on('mousedown.irs_' + this.plugin_count, this.pointerDown.bind(this, 'single')), this.$cache.shad_single.on('mousedown.irs_' + this.plugin_count, this.pointerClick.bind(this, 'click'))) : (this.$cache.s_from.on('touchstart.irs_' + this.plugin_count, this.pointerDown.bind(this, 'from')), this.$cache.s_to.on('touchstart.irs_' + this.plugin_count, this.pointerDown.bind(this, 'to')), this.$cache.shad_from.on('touchstart.irs_' + this.plugin_count, this.pointerClick.bind(this, 'click')), this.$cache.shad_to.on('touchstart.irs_' + this.plugin_count, this.pointerClick.bind(this, 'click')), this.$cache.s_from.on('mousedown.irs_' + this.plugin_count, this.pointerDown.bind(this, 'from')), this.$cache.s_to.on('mousedown.irs_' + this.plugin_count, this.pointerDown.bind(this, 'to')), this.$cache.shad_from.on('mousedown.irs_' + this.plugin_count, this.pointerClick.bind(this, 'click')), this.$cache.shad_to.on('mousedown.irs_' + this.plugin_count, this.pointerClick.bind(this, 'click')));
            if (this.options.keyboard) this.$cache.line.on('keydown.irs_' +
            this.plugin_count, this.key.bind(this, 'keyboard'));
            n && (this.$cache.body.on('mouseup.irs_' + this.plugin_count, this.pointerUp.bind(this)), this.$cache.body.on('mouseleave.irs_' + this.plugin_count, this.pointerUp.bind(this)))
        },
        pointerMove: function (a) {
            this.dragging && (this.coords.x_pointer = (a.pageX || a.originalEvent.touches && a.originalEvent.touches[0].pageX) - this.coords.x_gap, this.calc())
        },
        pointerUp: function (a) {
            if (this.current_plugin === this.plugin_count && this.is_active) {
                this.is_active = !1;
                var b = this.options.onFinish &&
                'function' === typeof this.options.onFinish;
                a = g.contains(this.$cache.cont[0], a.target) || this.dragging;
                if (b && a) this.options.onFinish(this.result);
                this.$cache.cont.find('.state_hover').removeClass('state_hover');
                this.force_redraw = !0;
                this.dragging = !1;
                n && g('*').prop('unselectable', !1);
                this.updateScene()
            }
        },
        pointerDown: function (a, b) {
            b.preventDefault();
            var c = b.pageX || b.originalEvent.touches && b.originalEvent.touches[0].pageX;
            if (2 !== b.button) {
                this.current_plugin = this.plugin_count;
                this.target = a;
                this.dragging =
                this.is_active = !0;
                this.coords.x_gap = this.$cache.rs.offset().left;
                this.coords.x_pointer = c - this.coords.x_gap;
                this.calcPointer();
                switch (a) {
                    case 'single':
                        this.coords.p_gap = this.toFixed(this.coords.p_pointer - this.coords.p_single);
                        break;
                    case 'from':
                        this.coords.p_gap = this.toFixed(this.coords.p_pointer - this.coords.p_from);
                        this.$cache.s_from.addClass('state_hover');
                        this.$cache.s_from.addClass('type_last');
                        this.$cache.s_to.removeClass('type_last');
                        break;
                    case 'to':
                        this.coords.p_gap = this.toFixed(this.coords.p_pointer -
                        this.coords.p_to);
                        this.$cache.s_to.addClass('state_hover');
                        this.$cache.s_to.addClass('type_last');
                        this.$cache.s_from.removeClass('type_last');
                        break;
                    case 'both':
                        this.coords.p_gap_left = this.toFixed(this.coords.p_pointer - this.coords.p_from),
                        this.coords.p_gap_right = this.toFixed(this.coords.p_to - this.coords.p_pointer),
                        this.$cache.s_to.removeClass('type_last'),
                        this.$cache.s_from.removeClass('type_last')
                }
                n && g('*').prop('unselectable', !0);
                this.$cache.line.trigger('focus');
                this.updateScene()
            }
        },
        pointerClick: function (a, b) {
            b.preventDefault();
            var c = b.pageX || b.originalEvent.touches && b.originalEvent.touches[0].pageX;
            2 !== b.button && (this.current_plugin = this.plugin_count, this.target = a, this.is_click = !0, this.coords.x_gap = this.$cache.rs.offset().left, this.coords.x_pointer = +(c - this.coords.x_gap).toFixed(), this.force_redraw = !0, this.calc(), this.$cache.line.trigger('focus'))
        },
        key: function (a, b) {
            if (!(this.current_plugin !== this.plugin_count || b.altKey || b.ctrlKey || b.shiftKey || b.metaKey)) {
                switch (b.which) {
                    case 83:
                    case 65:
                    case 40:
                    case 37:
                        b.preventDefault();
                        this.moveByKey(!1);
                        break;
                    case 87:
                    case 68:
                    case 38:
                    case 39:
                        b.preventDefault(),
                        this.moveByKey(!0)
                }
                return !0
            }
        },
        moveByKey: function (a) {
            var b = this.coords.p_pointer,
            b = a ? b + this.options.keyboard_step : b - this.options.keyboard_step;
            this.coords.x_pointer = this.toFixed(this.coords.w_rs / 100 * b);
            this.is_key = !0;
            this.calc()
        },
        setMinMax: function () {
            this.options && (this.options.hide_min_max ? (this.$cache.min[0].style.display = 'none', this.$cache.max[0].style.display = 'none') : (this.options.values.length ? (this.$cache.min.html(this.decorate(this.options.p_values[this.options.min])), this.$cache.max.html(this.decorate(this.options.p_values[this.options.max]))) : (this.$cache.min.html(this.decorate(this._prettify(this.options.min), this.options.min)), this.$cache.max.html(this.decorate(this._prettify(this.options.max), this.options.max))), this.labels.w_min = this.$cache.min.outerWidth(!1), this.labels.w_max = this.$cache.max.outerWidth(!1)))
        },
        calc: function (a) {
            if (this.options) {
                this.calc_count++;
                if (10 === this.calc_count || a) this.calc_count = 0,
                this.coords.w_rs = this.$cache.rs.outerWidth(!1),
                this.coords.w_handle =
                'single' === this.options.type ? this.$cache.s_single.outerWidth(!1) : this.$cache.s_from.outerWidth(!1);
                if (this.coords.w_rs) {
                    this.calcPointer();
                    this.coords.p_handle = this.toFixed(this.coords.w_handle / this.coords.w_rs * 100);
                    a = 100 - this.coords.p_handle;
                    var b = this.toFixed(this.coords.p_pointer - this.coords.p_gap);
                    'click' === this.target && (b = this.toFixed(this.coords.p_pointer - this.coords.p_handle / 2), this.target = this.chooseHandle(b));
                    0 > b ? b = 0 : b > a && (b = a);
                    switch (this.target) {
                        case 'base':
                            b = (this.options.max - this.options.min) /
                            100;
                            a = (this.result.from - this.options.min) / b;
                            b = (this.result.to - this.options.min) / b;
                            this.coords.p_single_real = this.toFixed(a);
                            this.coords.p_from_real = this.toFixed(a);
                            this.coords.p_to_real = this.toFixed(b);
                            this.coords.p_single_real = this.checkDiapason(this.coords.p_single_real, this.options.from_min, this.options.from_max);
                            this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real, this.options.from_min, this.options.from_max);
                            this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max);
                            this.coords.p_single = this.toFixed(a - this.coords.p_handle / 100 * a);
                            this.coords.p_from = this.toFixed(a - this.coords.p_handle / 100 * a);
                            this.coords.p_to = this.toFixed(b - this.coords.p_handle / 100 * b);
                            this.target = null;
                            break;
                        case 'single':
                            if (this.options.from_fixed) break;
                            this.coords.p_single_real = this.calcWithStep(b / a * 100);
                            this.coords.p_single_real = this.checkDiapason(this.coords.p_single_real, this.options.from_min, this.options.from_max);
                            this.coords.p_single = this.toFixed(this.coords.p_single_real /
                            100 * a);
                            break;
                        case 'from':
                            if (this.options.from_fixed) break;
                            this.coords.p_from_real = this.calcWithStep(b / a * 100);
                            this.coords.p_from_real > this.coords.p_to_real && (this.coords.p_from_real = this.coords.p_to_real);
                            this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real, this.options.from_min, this.options.from_max);
                            this.coords.p_from_real = this.checkMinInterval(this.coords.p_from_real, this.coords.p_to_real, 'from');
                            this.coords.p_from_real = this.checkMaxInterval(this.coords.p_from_real, this.coords.p_to_real, 'from');
                            this.coords.p_from = this.toFixed(this.coords.p_from_real / 100 * a);
                            break;
                        case 'to':
                            if (this.options.to_fixed) break;
                            this.coords.p_to_real = this.calcWithStep(b / a * 100);
                            this.coords.p_to_real < this.coords.p_from_real && (this.coords.p_to_real = this.coords.p_from_real);
                            this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max);
                            this.coords.p_to_real = this.checkMinInterval(this.coords.p_to_real, this.coords.p_from_real, 'to');
                            this.coords.p_to_real = this.checkMaxInterval(this.coords.p_to_real, this.coords.p_from_real, 'to');
                            this.coords.p_to = this.toFixed(this.coords.p_to_real / 100 * a);
                            break;
                        case 'both':
                            if (this.options.from_fixed || this.options.to_fixed) break;
                            b = this.toFixed(b + 0.1 * this.coords.p_handle);
                            this.coords.p_from_real = this.calcWithStep((b - this.coords.p_gap_left) / a * 100);
                            this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real, this.options.from_min, this.options.from_max);
                            this.coords.p_from_real = this.checkMinInterval(this.coords.p_from_real, this.coords.p_to_real, 'from');
                            this.coords.p_from =
                            this.toFixed(this.coords.p_from_real / 100 * a);
                            this.coords.p_to_real = this.calcWithStep((b + this.coords.p_gap_right) / a * 100);
                            this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max);
                            this.coords.p_to_real = this.checkMinInterval(this.coords.p_to_real, this.coords.p_from_real, 'to');
                            this.coords.p_to = this.toFixed(this.coords.p_to_real / 100 * a)
                    }
                    'single' === this.options.type ? (this.coords.p_bar_x = this.coords.p_handle / 2, this.coords.p_bar_w = this.coords.p_single, this.result.from_percent =
                    this.coords.p_single_real, this.result.from = this.calcReal(this.coords.p_single_real), this.options.values.length && (this.result.from_value = this.options.values[this.result.from])) : (this.coords.p_bar_x = this.toFixed(this.coords.p_from + this.coords.p_handle / 2), this.coords.p_bar_w = this.toFixed(this.coords.p_to - this.coords.p_from), this.result.from_percent = this.coords.p_from_real, this.result.from = this.calcReal(this.coords.p_from_real), this.result.to_percent = this.coords.p_to_real, this.result.to = this.calcReal(this.coords.p_to_real), this.options.values.length && (this.result.from_value = this.options.values[this.result.from], this.result.to_value = this.options.values[this.result.to]));
                    this.calcMinMax();
                    this.calcLabels()
                }
            }
        },
        calcPointer: function () {
            this.coords.w_rs ? (0 > this.coords.x_pointer || isNaN(this.coords.x_pointer) ? this.coords.x_pointer = 0 : this.coords.x_pointer > this.coords.w_rs && (this.coords.x_pointer = this.coords.w_rs), this.coords.p_pointer = this.toFixed(this.coords.x_pointer / this.coords.w_rs * 100)) : this.coords.p_pointer = 0
        },
        chooseHandle: function (a) {
            return 'single' ===
            this.options.type ? 'single' : a >= this.coords.p_from_real + (this.coords.p_to_real - this.coords.p_from_real) / 2 ? this.options.to_fixed ? 'from' : 'to' : this.options.from_fixed ? 'to' : 'from'
        },
        calcMinMax: function () {
            this.coords.w_rs && (this.labels.p_min = this.labels.w_min / this.coords.w_rs * 100, this.labels.p_max = this.labels.w_max / this.coords.w_rs * 100)
        },
        calcLabels: function () {
            this.coords.w_rs && !this.options.hide_from_to && ('single' === this.options.type ? (this.labels.w_single = this.$cache.single.outerWidth(!1), this.labels.p_single =
            this.labels.w_single / this.coords.w_rs * 100, this.labels.p_single_left = this.coords.p_single + this.coords.p_handle / 2 - this.labels.p_single / 2) : (this.labels.w_from = this.$cache.from.outerWidth(!1), this.labels.p_from = this.labels.w_from / this.coords.w_rs * 100, this.labels.p_from_left = this.coords.p_from + this.coords.p_handle / 2 - this.labels.p_from / 2, this.labels.p_from_left = this.toFixed(this.labels.p_from_left), this.labels.p_from_left = this.checkEdges(this.labels.p_from_left, this.labels.p_from), this.labels.w_to = this.$cache.to.outerWidth(!1), this.labels.p_to = this.labels.w_to / this.coords.w_rs * 100, this.labels.p_to_left = this.coords.p_to + this.coords.p_handle / 2 - this.labels.p_to / 2, this.labels.p_to_left = this.toFixed(this.labels.p_to_left), this.labels.p_to_left = this.checkEdges(this.labels.p_to_left, this.labels.p_to), this.labels.w_single = this.$cache.single.outerWidth(!1), this.labels.p_single = this.labels.w_single / this.coords.w_rs * 100, this.labels.p_single_left = (this.labels.p_from_left + this.labels.p_to_left + this.labels.p_to) / 2 - this.labels.p_single /
            2, this.labels.p_single_left = this.toFixed(this.labels.p_single_left)), this.labels.p_single_left = this.checkEdges(this.labels.p_single_left, this.labels.p_single))
        },
        updateScene: function () {
            this.raf_id && (cancelAnimationFrame(this.raf_id), this.raf_id = null);
            clearTimeout(this.update_tm);
            this.update_tm = null;
            this.options && (this.drawHandles(), this.is_active ? this.raf_id = requestAnimationFrame(this.updateScene.bind(this)) : this.update_tm = setTimeout(this.updateScene.bind(this), 300))
        },
        drawHandles: function () {
            this.coords.w_rs =
            this.$cache.rs.outerWidth(!1);
            if (this.coords.w_rs) {
                this.coords.w_rs !== this.coords.w_rs_old && (this.target = 'base', this.is_resize = !0);
                if (this.coords.w_rs !== this.coords.w_rs_old || this.force_redraw) this.setMinMax(),
                this.calc(!0),
                this.drawLabels(),
                this.options.grid && (this.calcGridMargin(), this.calcGridLabels()),
                this.force_redraw = !0,
                this.coords.w_rs_old = this.coords.w_rs,
                this.drawShadow();
                if (this.coords.w_rs && (this.dragging || this.force_redraw || this.is_key)) {
                    if (this.old_from !== this.result.from || this.old_to !==
                    this.result.to || this.force_redraw || this.is_key) {
                        this.drawLabels();
                        this.$cache.bar[0].style.left = this.coords.p_bar_x + '%';
                        this.$cache.bar[0].style.width = this.coords.p_bar_w + '%';
                        if ('single' === this.options.type) this.$cache.s_single[0].style.left = this.coords.p_single + '%',
                        this.$cache.single[0].style.left = this.labels.p_single_left + '%',
                        this.options.values.length ? (this.$cache.input.prop('value', this.result.from_value), this.$cache.input.data('from', this.result.from_value)) : (this.$cache.input.prop('value', this.result.from), this.$cache.input.data('from', this.result.from));
                        else {
                            this.$cache.s_from[0].style.left = this.coords.p_from + '%';
                            this.$cache.s_to[0].style.left = this.coords.p_to + '%';
                            if (this.old_from !== this.result.from || this.force_redraw) this.$cache.from[0].style.left = this.labels.p_from_left + '%';
                            if (this.old_to !== this.result.to || this.force_redraw) this.$cache.to[0].style.left = this.labels.p_to_left + '%';
                            this.$cache.single[0].style.left = this.labels.p_single_left + '%';
                            this.options.values.length ? (this.$cache.input.prop('value', this.result.from_value + ';' + this.result.to_value), this.$cache.input.data('from', this.result.from_value), this.$cache.input.data('to', this.result.to_value)) : (this.$cache.input.prop('value', this.result.from + ';' + this.result.to), this.$cache.input.data('from', this.result.from), this.$cache.input.data('to', this.result.to))
                        }
                        this.old_from === this.result.from && this.old_to === this.result.to || this.is_start || this.$cache.input.trigger('change');
                        this.old_from = this.result.from;
                        this.old_to = this.result.to;
                        if (this.options.onChange &&
                        'function' === typeof this.options.onChange && !this.is_resize && !this.is_update && !this.is_start) this.options.onChange(this.result);
                        if (this.options.onFinish && 'function' === typeof this.options.onFinish && (this.is_key || this.is_click)) this.options.onFinish(this.result);
                        this.is_resize = this.is_update = !1
                    }
                    this.force_redraw = this.is_click = this.is_key = this.is_start = !1
                }
            }
        },
        drawLabels: function () {
            if (this.options) {
                var a = this.options.values.length,
                b = this.options.p_values,
                c;
                if (!this.options.hide_from_to) if ('single' === this.options.type) a =
                a ? this.decorate(b[this.result.from]) : this.decorate(this._prettify(this.result.from), this.result.from),
                this.$cache.single.html(a),
                this.calcLabels(),
                this.$cache.min[0].style.visibility = this.labels.p_single_left < this.labels.p_min + 1 ? 'hidden' : 'visible',
                this.$cache.max[0].style.visibility = this.labels.p_single_left + this.labels.p_single > 100 - this.labels.p_max - 1 ? 'hidden' : 'visible';
                else {
                    a ? (this.options.decorate_both ? (a = this.decorate(b[this.result.from]), a += this.options.values_separator, a += this.decorate(b[this.result.to])) :
                    a = this.decorate(b[this.result.from] + this.options.values_separator + b[this.result.to]), c = this.decorate(b[this.result.from]), b = this.decorate(b[this.result.to])) : (this.options.decorate_both ? (a = this.decorate(this._prettify(this.result.from), this.result.from), a += this.options.values_separator, a += this.decorate(this._prettify(this.result.to), this.result.to)) : a = this.decorate(this._prettify(this.result.from) + this.options.values_separator + this._prettify(this.result.to), this.result.to), c = this.decorate(this._prettify(this.result.from), this.result.from), b = this.decorate(this._prettify(this.result.to), this.result.to));
                    this.$cache.single.html(a);
                    this.$cache.from.html(c);
                    this.$cache.to.html(b);
                    this.calcLabels();
                    b = Math.min(this.labels.p_single_left, this.labels.p_from_left);
                    a = this.labels.p_single_left + this.labels.p_single;
                    c = this.labels.p_to_left + this.labels.p_to;
                    var d = Math.max(a, c);
                    this.labels.p_from_left + this.labels.p_from >= this.labels.p_to_left ? (this.$cache.from[0].style.visibility = 'hidden', this.$cache.to[0].style.visibility = 'hidden', this.$cache.single[0].style.visibility = 'visible', this.result.from === this.result.to ? (this.$cache.from[0].style.visibility = 'visible', this.$cache.single[0].style.visibility = 'hidden', d = c) : (this.$cache.from[0].style.visibility = 'hidden', this.$cache.single[0].style.visibility = 'visible', d = Math.max(a, c))) : (this.$cache.from[0].style.visibility = 'visible', this.$cache.to[0].style.visibility = 'visible', this.$cache.single[0].style.visibility = 'hidden');
                    this.$cache.min[0].style.visibility = b < this.labels.p_min + 1 ? 'hidden' :
                    'visible';
                    this.$cache.max[0].style.visibility = d > 100 - this.labels.p_max - 1 ? 'hidden' : 'visible'
                }
            }
        },
        drawShadow: function () {
            var a = this.options,
            b = this.$cache,
            c = 'number' === typeof a.from_min && !isNaN(a.from_min),
            d = 'number' === typeof a.from_max && !isNaN(a.from_max),
            e = 'number' === typeof a.to_min && !isNaN(a.to_min),
            f = 'number' === typeof a.to_max && !isNaN(a.to_max);
            'single' === a.type ? a.from_shadow && (c || d) ? (c = this.calcPercent(a.from_min || a.min), d = this.calcPercent(a.from_max || a.max) - c, c = this.toFixed(c - this.coords.p_handle /
            100 * c), d = this.toFixed(d - this.coords.p_handle / 100 * d), c += this.coords.p_handle / 2, b.shad_single[0].style.display = 'block', b.shad_single[0].style.left = c + '%', b.shad_single[0].style.width = d + '%') : b.shad_single[0].style.display = 'none' : (a.from_shadow && (c || d) ? (c = this.calcPercent(a.from_min || a.min), d = this.calcPercent(a.from_max || a.max) - c, c = this.toFixed(c - this.coords.p_handle / 100 * c), d = this.toFixed(d - this.coords.p_handle / 100 * d), c += this.coords.p_handle / 2, b.shad_from[0].style.display = 'block', b.shad_from[0].style.left =
            c + '%', b.shad_from[0].style.width = d + '%') : b.shad_from[0].style.display = 'none', a.to_shadow && (e || f) ? (e = this.calcPercent(a.to_min || a.min), a = this.calcPercent(a.to_max || a.max) - e, e = this.toFixed(e - this.coords.p_handle / 100 * e), a = this.toFixed(a - this.coords.p_handle / 100 * a), e += this.coords.p_handle / 2, b.shad_to[0].style.display = 'block', b.shad_to[0].style.left = e + '%', b.shad_to[0].style.width = a + '%') : b.shad_to[0].style.display = 'none')
        },
        toggleInput: function () {
            this.$cache.input.toggleClass('irs-hidden-input')
        },
        calcPercent: function (a) {
            return this.toFixed((a -
            this.options.min) / ((this.options.max - this.options.min) / 100))
        },
        calcReal: function (a) {
            var b = this.options.min,
            c = this.options.max,
            d = 0;
            0 > b && (d = Math.abs(b), b += d, c += d);
            a = (c - b) / 100 * a + b;
            (b = this.options.step.toString().split('.')[1]) ? a = +a.toFixed(b.length) : (a /= this.options.step, a *= this.options.step, a = +a.toFixed(0));
            d && (a -= d);
            d = b ? +a.toFixed(b.length) : this.toFixed(a);
            d < this.options.min ? d = this.options.min : d > this.options.max && (d = this.options.max);
            return d
        },
        calcWithStep: function (a) {
            var b = Math.round(a / this.coords.p_step) *
            this.coords.p_step;
            100 < b && (b = 100);
            100 === a && (b = 100);
            return this.toFixed(b)
        },
        checkMinInterval: function (a, b, c) {
            var d = this.options;
            if (!d.min_interval) return a;
            a = this.calcReal(a);
            b = this.calcReal(b);
            'from' === c ? b - a < d.min_interval && (a = b - d.min_interval) : a - b < d.min_interval && (a = b + d.min_interval);
            return this.calcPercent(a)
        },
        checkMaxInterval: function (a, b, c) {
            var d = this.options;
            if (!d.max_interval) return a;
            a = this.calcReal(a);
            b = this.calcReal(b);
            'from' === c ? b - a > d.max_interval && (a = b - d.max_interval) : a - b > d.max_interval && (a = b + d.max_interval);
            return this.calcPercent(a)
        },
        checkDiapason: function (a, b, c) {
            a = this.calcReal(a);
            var d = this.options;
            b && 'number' === typeof b || (b = d.min);
            c && 'number' === typeof c || (c = d.max);
            a < b && (a = b);
            a > c && (a = c);
            return this.calcPercent(a)
        },
        toFixed: function (a) {
            a = a.toFixed(5);
            return +a
        },
        _prettify: function (a) {
            return this.options.prettify_enabled ? this.options.prettify && 'function' === typeof this.options.prettify ? this.options.prettify(a) : this.prettify(a) : a
        },
        prettify: function (a) {
            return a.toString().replace(/(\d{1,3}(?=(?:\d\d\d)+(?!\d)))/g, '$1' + this.options.prettify_separator)
        },
        checkEdges: function (a, b) {
            if (!this.options.force_edges) return this.toFixed(a);
            0 > a ? a = 0 : a > 100 - b && (a = 100 - b);
            return this.toFixed(a)
        },
        validate: function () {
            var a = this.options,
            b = this.result,
            c = a.values,
            d = c.length,
            e,
            f;
            'string' === typeof a.min && (a.min = +a.min);
            'string' === typeof a.max && (a.max = +a.max);
            'string' === typeof a.from && (a.from = +a.from);
            'string' === typeof a.to && (a.to = +a.to);
            'string' === typeof a.step && (a.step = +a.step);
            'string' === typeof a.from_min && (a.from_min = +a.from_min);
            'string' === typeof a.from_max && (a.from_max = +a.from_max);
            'string' === typeof a.to_min && (a.to_min = +a.to_min);
            'string' === typeof a.to_max && (a.to_max = +a.to_max);
            'string' === typeof a.keyboard_step && (a.keyboard_step = +a.keyboard_step);
            'string' === typeof a.grid_num && (a.grid_num = +a.grid_num);
            a.max <= a.min && (a.max = a.min ? 2 * a.min : a.min + 1, a.step = 1);
            if (d) for (a.p_values = [
            ], a.min = 0, a.max = d - 1, a.step = 1, a.grid_num = a.max, a.grid_snap = !0, f = 0; f < d; f++) e = +c[f],
            isNaN(e) ? e = c[f] : (c[f] = e, e = this._prettify(e)),
            a.p_values.push(e);
            if ('number' !==
            typeof a.from || isNaN(a.from)) a.from = a.min;
            if ('number' !== typeof a.to || isNaN(a.from)) a.to = a.max;
            if ('single' === a.type) a.from < a.min && (a.from = a.min),
            a.from > a.max && (a.from = a.max);
            else {
                if (a.from < a.min || a.from > a.max) a.from = a.min;
                if (a.to > a.max || a.to < a.min) a.to = a.max;
                a.from > a.to && (a.from = a.to)
            }
            if ('number' !== typeof a.step || isNaN(a.step) || !a.step || 0 > a.step) a.step = 1;
            if ('number' !== typeof a.keyboard_step || isNaN(a.keyboard_step) || !a.keyboard_step || 0 > a.keyboard_step) a.keyboard_step = 5;
            a.from_min && a.from < a.from_min && (a.from = a.from_min);
            a.from_max && a.from > a.from_max && (a.from = a.from_max);
            a.to_min && a.to < a.to_min && (a.to = a.to_min);
            a.to_max && a.from > a.to_max && (a.to = a.to_max);
            if (b) {
                b.min !== a.min && (b.min = a.min);
                b.max !== a.max && (b.max = a.max);
                if (b.from < b.min || b.from > b.max) b.from = a.from;
                if (b.to < b.min || b.to > b.max) b.to = a.to
            }
            if ('number' !== typeof a.min_interval || isNaN(a.min_interval) || !a.min_interval || 0 > a.min_interval) a.min_interval = 0;
            if ('number' !== typeof a.max_interval || isNaN(a.max_interval) || !a.max_interval || 0 > a.max_interval) a.max_interval =
            0;
            a.min_interval && a.min_interval > a.max - a.min && (a.min_interval = a.max - a.min);
            a.max_interval && a.max_interval > a.max - a.min && (a.max_interval = a.max - a.min)
        },
        decorate: function (a, b) {
            var c = '',
            d = this.options;
            d.prefix && (c += d.prefix);
            c += a;
            d.max_postfix && (d.values.length && a === d.p_values[d.max] ? (c += d.max_postfix, d.postfix && (c += ' ')) : b === d.max && (c += d.max_postfix, d.postfix && (c += ' ')));
            d.postfix && (c += d.postfix);
            return c
        },
        updateFrom: function () {
            this.result.from = this.options.from;
            this.result.from_percent = this.calcPercent(this.result.from);
            this.options.values && (this.result.from_value = this.options.values[this.result.from])
        },
        updateTo: function () {
            this.result.to = this.options.to;
            this.result.to_percent = this.calcPercent(this.result.to);
            this.options.values && (this.result.to_value = this.options.values[this.result.to])
        },
        updateResult: function () {
            this.result.min = this.options.min;
            this.result.max = this.options.max;
            this.updateFrom();
            this.updateTo()
        },
        appendGrid: function () {
            if (this.options.grid) {
                var a = this.options,
                b,
                c;
                b = a.max - a.min;
                var d = a.grid_num,
                e = 0,
                f =
                0,
                k = 4,
                g,
                h,
                l = 0,
                m = '';
                this.calcGridMargin();
                a.grid_snap ? (d = b / a.step, e = this.toFixed(a.step / (b / 100))) : e = this.toFixed(100 / d);
                4 < d && (k = 3);
                7 < d && (k = 2);
                14 < d && (k = 1);
                28 < d && (k = 0);
                for (b = 0; b < d + 1; b++) {
                    g = k;
                    f = this.toFixed(e * b);
                    100 < f && (f = 100, g -= 2, 0 > g && (g = 0));
                    this.coords.big[b] = f;
                    h = (f - e * (b - 1)) / (g + 1);
                    for (c = 1; c <= g && 0 !== f; c++) l = this.toFixed(f - h * c),
                    m += '<span class="irs-grid-pol small" style="left: ' + l + '%"></span>';
                    m += '<span class="irs-grid-pol" style="left: ' + f + '%"></span>';
                    l = this.calcReal(f);
                    l = a.values.length ? a.p_values[l] :
                    this._prettify(l);
                    m += '<span class="irs-grid-text js-grid-text-' + b + '" style="left: ' + f + '%">' + l + '</span>'
                }
                this.coords.big_num = Math.ceil(d + 1);
                this.$cache.cont.addClass('irs-with-grid');
                this.$cache.grid.html(m);
                this.cacheGridLabels()
            }
        },
        cacheGridLabels: function () {
            var a,
            b,
            c = this.coords.big_num;
            for (b = 0; b < c; b++) a = this.$cache.grid.find('.js-grid-text-' + b),
            this.$cache.grid_labels.push(a);
            this.calcGridLabels()
        },
        calcGridLabels: function () {
            var a,
            b;
            b = [
            ];
            var c = [
            ],
            d = this.coords.big_num;
            for (a = 0; a < d; a++) this.coords.big_w[a] =
            this.$cache.grid_labels[a].outerWidth(!1),
            this.coords.big_p[a] = this.toFixed(this.coords.big_w[a] / this.coords.w_rs * 100),
            this.coords.big_x[a] = this.toFixed(this.coords.big_p[a] / 2),
            b[a] = this.toFixed(this.coords.big[a] - this.coords.big_x[a]),
            c[a] = this.toFixed(b[a] + this.coords.big_p[a]);
            this.options.force_edges && (b[0] < -this.coords.grid_gap && (b[0] = -this.coords.grid_gap, c[0] = this.toFixed(b[0] + this.coords.big_p[0]), this.coords.big_x[0] = this.coords.grid_gap), c[d - 1] > 100 + this.coords.grid_gap && (c[d - 1] = 100 + this.coords.grid_gap, b[d - 1] = this.toFixed(c[d - 1] - this.coords.big_p[d - 1]), this.coords.big_x[d - 1] = this.toFixed(this.coords.big_p[d - 1] - this.coords.grid_gap)));
            this.calcGridCollision(2, b, c);
            this.calcGridCollision(4, b, c);
            for (a = 0; a < d; a++) b = this.$cache.grid_labels[a][0],
            b.style.marginLeft = -this.coords.big_x[a] + '%'
        },
        calcGridCollision: function (a, b, c) {
            var d,
            e,
            f,
            g = this.coords.big_num;
            for (d = 0; d < g; d += a) {
                e = d + a / 2;
                if (e >= g) break;
                f = this.$cache.grid_labels[e][0];
                f.style.visibility = c[d] <= b[e] ? 'visible' : 'hidden'
            }
        },
        calcGridMargin: function () {
            this.options.grid_margin && (this.coords.w_rs = this.$cache.rs.outerWidth(!1), this.coords.w_rs && (this.coords.w_handle = 'single' === this.options.type ? this.$cache.s_single.outerWidth(!1) : this.$cache.s_from.outerWidth(!1), this.coords.p_handle = this.toFixed(this.coords.w_handle / this.coords.w_rs * 100), this.coords.grid_gap = this.toFixed(this.coords.p_handle / 2 - 0.1), this.$cache.grid[0].style.width = this.toFixed(100 - this.coords.p_handle) + '%', this.$cache.grid[0].style.left = this.coords.grid_gap + '%'))
        },
        update: function (a) {
            this.input && (this.is_update =
            !0, this.options.from = this.result.from, this.options.to = this.result.to, this.options = g.extend(this.options, a), this.validate(), this.updateResult(a), this.toggleInput(), this.remove(), this.init(!0))
        },
        reset: function () {
            this.input && (this.updateResult(), this.update())
        },
        destroy: function () {
            this.input && (this.toggleInput(), this.$cache.input.prop('readonly', !1), g.data(this.input, 'ionRangeSlider', null), this.remove(), this.options = this.input = null)
        }
    };
    g.fn.ionRangeSlider = function (a) {
        return this.each(function () {
            g.data(this, 'ionRangeSlider') || g.data(this, 'ionRangeSlider', new p(this, a, t++))
        })
    };
    (function () {
        for (var a = 0, b = [
          'ms',
          'moz',
          'webkit',
          'o'
        ], c = 0; c < b.length && !h.requestAnimationFrame; ++c) h.requestAnimationFrame = h[b[c] + 'RequestAnimationFrame'],
        h.cancelAnimationFrame = h[b[c] + 'CancelAnimationFrame'] || h[b[c] + 'CancelRequestAnimationFrame'];
        h.requestAnimationFrame || (h.requestAnimationFrame = function (b, c) {
            var f = (new Date).getTime(),
            g = Math.max(0, 16 - (f - a)),
            n = h.setTimeout(function () {
                b(f + g)
            }, g);
            a = f + g;
            return n
        });
        h.cancelAnimationFrame || (h.cancelAnimationFrame = function (a) {
            clearTimeout(a)
        })
    })()
})(jQuery, document, window, navigator);
