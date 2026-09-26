/* Dev·stopian — Keep the Edge
 *
 * You are a developer adrift in the AI flood. Tokens (AI output) raise your
 * score but dull your edge. Rough ideas and writing sharpen it again. Sharp
 * devs catch hallucinations; dull ones get fooled by them, because to a dull
 * eye a hallucination looks just like real output.
 */
(() => {
  'use strict';

  // ---------- constants ----------
  const C = {
    void: '#0c0f1a', plasma: '#38d9e0', plasmaLight: '#9eeef0',
    signal: '#f2a73a', signalLight: '#f8d49a', nebula: '#ec5fa6',
    quasar: '#8c6ee6', mist: '#c8cfdb', haze: '#8e97a9', neg: '#e5484d', star: '#e8ecf4',
  };

  const START_YEAR = 2022;
  const YEAR_SECONDS = 35;
  const EDGE_MAX = 100;
  const START_EDGE = 70;

  const YEAR_LINES = {
    2022: 'Generative AI arrives. The future looks dev·stopian.',
    2023: 'The acceleration: it writes code that took you years to master.',
    2024: 'Think, prompt, read+, build, verify.',
    2025: 'GenAI pushed us all to be readers.',
    2026: 'Writing needs to be rough. How else can we keep the edge?',
  };
  const FUTURE_LINES = [
    'Beyond the roadmap.', 'The flood never recedes.', 'Still writing. Still rough.',
    'Everything is an artifact.', 'Read, read, read. Then write.',
  ];

  const TOKEN_GLYPHS = ['fn', '=>', '{}', 'if', '[]', '::', '&&', '()', '<>', '//', '0x', 'λ', '++', '!=', '$_', '#', 'ok', '?.'];

  // Perks are named after posts from the old blog.
  const PERKS = [
    { id: 'immutability', name: 'Immutability', desc: 'Tokens dull your edge 25% less.', tag: 'archive · 2019', max: 2 },
    { id: 'testgen', name: 'Test Gen', desc: 'Hallucinations move 25% slower.', tag: 'archive · 2019', max: 2 },
    { id: 'gitflow', name: 'Gitflow', desc: 'Writing restores edge 50% faster.', tag: 'archive · 2021', max: 2 },
    { id: 'docker', name: 'Dockerized Dev', desc: '+1 max trust, and restore 1 trust.', tag: 'archive · 2021', max: 2 },
    { id: 'runscript', name: 'Run Script', desc: 'Tokens are worth 30% more.', tag: 'archive · 2023', max: 2 },
    { id: 'devstack', name: 'Devstack', desc: 'You count as sharp from 40 edge instead of 50.', tag: 'archive · 2024', max: 1 },
    { id: 'gateway', name: 'Resilient Gateway', desc: 'A shield blocks one hit every year.', tag: 'archive · 2024', max: 1 },
    { id: 'promptme', name: 'PromptMe', desc: 'Writing barely slows you down.', tag: 'archive · 2026', max: 1 },
    { id: 'techstack', name: 'Tech Stack', desc: 'Rough ideas restore 50% more edge.', tag: 'archive · 2026', max: 2 },
    { id: 'stopvibe', name: 'Stop Vibe to Five', desc: 'Your edge fades half as fast on its own.', tag: 'archive · 2026', max: 1 },
    { id: 'agentic', name: 'Agentic Demos', desc: 'Nearby tokens drift toward you.', tag: 'archive · 2026', max: 2 },
    { id: 'mvdb', name: 'Multi-Version DB', desc: 'Once per run, roll back to 40 edge instead of going smooth.', tag: 'archive · 2026', max: 1 },
    { id: 'rust', name: 'Rust Scratch', desc: '+15% ship speed.', tag: 'archive · 2026', max: 2 },
    { id: 'artifact', name: 'Everything is an Artifact', desc: 'Rough ideas show up 40% more often.', tag: 'archive · 2026', max: 2 },
  ];

  // ---------- helpers ----------
  const rand = (a, b) => a + Math.random() * (b - a);
  const pick = arr => arr[Math.floor(Math.random() * arr.length)];
  const clamp = (v, a, b) => Math.max(a, Math.min(b, v));
  const dist2 = (a, b) => (a.x - b.x) ** 2 + (a.y - b.y) ** 2;
  const hexRgb = h => [1, 3, 5].map(i => parseInt(h.slice(i, i + 2), 16));
  const mix = (h1, h2, t, a = 1) => {
    const x = hexRgb(h1), y = hexRgb(h2);
    const c = x.map((v, i) => Math.round(v + (y[i] - v) * t));
    return `rgba(${c[0]},${c[1]},${c[2]},${a})`;
  };
  const store = {
    get(k, d) { try { const v = localStorage.getItem(k); return v === null ? d : JSON.parse(v); } catch { return d; } },
    set(k, v) { try { localStorage.setItem(k, JSON.stringify(v)); } catch { /* storage unavailable */ } },
  };
  const reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
  const coarsePointer = window.matchMedia('(pointer: coarse)').matches;

  // ---------- DOM ----------
  const $ = id => document.getElementById(id);
  const canvas = $('stage');
  const ctx = canvas.getContext('2d');
  const ui = {
    hud: $('hud'), year: $('hud-year'), yearBar: $('hud-year-bar'), score: $('hud-score'), mult: $('hud-mult'),
    edge: $('edge'), edgeFill: $('edge-fill'), edgeState: $('edge-state'), notch: document.querySelector('.edge__notch'),
    trust: $('trust'), mute: $('btn-mute'), pause: $('btn-pause'), write: $('btn-write'), banner: $('banner'),
    title: $('screen-title'), pauseScreen: $('screen-pause'), over: $('screen-over'),
  };

  // ---------- audio ----------
  const Sound = {
    ac: null,
    muted: store.get('devstopian.muted', false),
    init() {
      if (this.ac) return;
      try { this.ac = new (window.AudioContext || window.webkitAudioContext)(); } catch { this.ac = null; }
    },
    tone(freq, dur, type = 'sine', vol = 0.05, slideTo = null, delay = 0) {
      if (this.muted || !this.ac) return;
      const t0 = this.ac.currentTime + delay;
      const o = this.ac.createOscillator();
      const g = this.ac.createGain();
      o.type = type;
      o.frequency.setValueAtTime(freq, t0);
      if (slideTo) o.frequency.exponentialRampToValueAtTime(slideTo, t0 + dur);
      g.gain.setValueAtTime(vol, t0);
      g.gain.exponentialRampToValueAtTime(0.0001, t0 + dur);
      o.connect(g).connect(this.ac.destination);
      o.start(t0);
      o.stop(t0 + dur + 0.02);
    },
    token(edge) { this.tone(420 + edge * 4, 0.06, 'sine', 0.045); },
    shard() { this.tone(880, 0.12, 'triangle', 0.06); this.tone(1320, 0.18, 'triangle', 0.05, null, 0.06); },
    caught() { this.tone(1400, 0.08, 'square', 0.03); this.tone(300, 0.25, 'sawtooth', 0.04, 90); },
    hit() { this.tone(180, 0.4, 'sawtooth', 0.08, 50); },
    block() { this.tone(600, 0.2, 'triangle', 0.06, 1200); },
    key() { this.tone(rand(1700, 2600), 0.012, 'square', 0.012); },
    year() { [523, 659, 784, 1047].forEach((f, i) => this.tone(f, 0.18, 'triangle', 0.05, null, i * 0.08)); },
    over() { [392, 330, 262, 196].forEach((f, i) => this.tone(f, 0.3, 'sine', 0.06, null, i * 0.14)); },
  };

  // ---------- canvas & background ----------
  let W = 0, H = 0, DPR = 1;
  let stars = [];

  function resize() {
    DPR = Math.min(window.devicePixelRatio || 1, 2);
    W = window.innerWidth;
    H = window.innerHeight;
    canvas.width = Math.round(W * DPR);
    canvas.height = Math.round(H * DPR);
    ctx.setTransform(DPR, 0, 0, DPR, 0, 0);
    const n = Math.round((W * H) / 5200);
    stars = Array.from({ length: n }, () => ({
      x: Math.random() * W, y: Math.random() * H,
      layer: Math.random() < 0.6 ? 0 : Math.random() < 0.75 ? 1 : 2,
      tw: Math.random() * Math.PI * 2,
    }));
    if (G) {
      G.player.x = clamp(G.player.x, 0, W);
      G.player.y = clamp(G.player.y, 0, H);
    }
  }

  const STAR_LAYERS = [
    { speed: 4, size: 0.8, alpha: 0.45, par: 0.01 },
    { speed: 10, size: 1.2, alpha: 0.7, par: 0.025 },
    { speed: 22, size: 1.8, alpha: 0.95, par: 0.05 },
  ];

  function updateStars(dt) {
    for (const s of stars) {
      s.x -= STAR_LAYERS[s.layer].speed * dt;
      if (s.x < -4) { s.x = W + 4; s.y = Math.random() * H; }
      s.tw += dt * 2;
    }
  }

  function drawBackground(t) {
    ctx.fillStyle = C.void;
    ctx.fillRect(0, 0, W, H);
    // nebulae
    const g1 = ctx.createRadialGradient(W * 0.15, H * 0.1, 0, W * 0.15, H * 0.1, Math.max(W, H) * 0.55);
    g1.addColorStop(0, 'rgba(140,110,230,0.16)');
    g1.addColorStop(1, 'rgba(140,110,230,0)');
    ctx.fillStyle = g1;
    ctx.fillRect(0, 0, W, H);
    const g2 = ctx.createRadialGradient(W * 0.9, H * 0.95, 0, W * 0.9, H * 0.95, Math.max(W, H) * 0.5);
    g2.addColorStop(0, 'rgba(56,217,224,0.08)');
    g2.addColorStop(1, 'rgba(56,217,224,0)');
    ctx.fillStyle = g2;
    ctx.fillRect(0, 0, W, H);

    const px = G && state !== 'title' ? G.player.x - W / 2 : 0;
    const py = G && state !== 'title' ? G.player.y - H / 2 : 0;
    ctx.fillStyle = C.star;
    for (const s of stars) {
      const L = STAR_LAYERS[s.layer];
      ctx.globalAlpha = L.alpha * (0.65 + 0.35 * Math.sin(s.tw));
      const x = s.x - px * L.par, y = s.y - py * L.par;
      ctx.fillRect(x, y, L.size, L.size);
    }
    ctx.globalAlpha = 1;
  }

  // ---------- game state ----------
  let state = 'title'; // title | play | pause | over
  let G = null;
  let best = store.get('devstopian.best', 0);

  const SHIP_OUTLINE = [[20, 0], [-4, -8], [-12, -13], [-8, 0], [-12, 13], [-4, 8]];
  const SHIP_NOISE = Array.from({ length: SHIP_OUTLINE.length * 3 }, () => rand(-1, 1));

  function newGame(attract = false) {
    return {
      attract,
      t: 0, yearT: 0, year: START_YEAR,
      score: 0, edge: START_EDGE, trust: 3, maxTrust: 3, shield: 0,
      player: { x: W / 2, y: H / 2, vx: 0, vy: 0, r: 12, angle: -Math.PI / 2, inv: 0 },
      tokens: [], shards: [], halls: [], parts: [], texts: [],
      timers: { token: 0.4, shard: 2.2, hall: 7, key: 0 },
      perks: {}, mvdbUsed: false, writing: false,
      stats: { tokens: 0, shards: 0, caught: 0, hits: 0, writeTime: 0 },
      shake: 0,
    };
  }

  const perk = id => G.perks[id] || 0;
  const lvl = () => G.year - START_YEAR;
  const sharpAt = () => (perk('devstack') ? 40 : 50);
  const isSharp = () => G.edge >= sharpAt();
  const multiplier = () => (1 + G.edge / 40) * (1 + 0.3 * perk('runscript'));

  // ---------- input ----------
  const keys = new Set();
  const input = {
    target: null,       // absolute mouse target
    touch: null,        // { id, fx, fy, sx, sy } relative drag
    mouseWrite: false,
    keyWrite: false,
    btnWrite: false,
  };

  const DIR_KEYS = {
    ArrowUp: [0, -1], KeyW: [0, -1], ArrowDown: [0, 1], KeyS: [0, 1],
    ArrowLeft: [-1, 0], KeyA: [-1, 0], ArrowRight: [1, 0], KeyD: [1, 0],
  };

  window.addEventListener('keydown', e => {
    Sound.init();
    if (DIR_KEYS[e.code]) { keys.add(e.code); input.target = null; e.preventDefault(); }
    if (e.code === 'Space') { input.keyWrite = true; e.preventDefault(); }
    if (e.repeat) return;
    if (e.code === 'Enter') {
      if (state === 'title' || state === 'over') { e.preventDefault(); startGame(); }
    }
    if (e.code === 'KeyP' || e.code === 'Escape') {
      if (state === 'play') pauseGame();
      else if (state === 'pause') resumeGame();
    }
    if (e.code === 'KeyM') toggleMute();
  });
  window.addEventListener('keyup', e => {
    keys.delete(e.code);
    if (e.code === 'Space') input.keyWrite = false;
  });

  canvas.addEventListener('pointerdown', e => {
    Sound.init();
    if (e.pointerType === 'mouse') {
      input.target = { x: e.clientX, y: e.clientY };
      if (e.button === 0) input.mouseWrite = true;
    } else if (!input.touch && G) {
      input.touch = { id: e.pointerId, fx: e.clientX, fy: e.clientY, sx: G.player.x, sy: G.player.y };
      input.target = null;
    }
  });
  canvas.addEventListener('pointermove', e => {
    if (e.pointerType === 'mouse') input.target = { x: e.clientX, y: e.clientY };
    else if (input.touch && input.touch.id === e.pointerId) { input.touch.fx2 = e.clientX; input.touch.fy2 = e.clientY; }
  });
  const endPointer = e => {
    if (e.pointerType === 'mouse') input.mouseWrite = false;
    else if (input.touch && input.touch.id === e.pointerId) input.touch = null;
  };
  window.addEventListener('pointerup', endPointer);
  window.addEventListener('pointercancel', endPointer);
  canvas.addEventListener('contextmenu', e => e.preventDefault());

  ui.write.addEventListener('pointerdown', e => { e.preventDefault(); e.stopPropagation(); Sound.init(); input.btnWrite = true; ui.write.classList.add('is-down'); });
  const releaseWrite = () => { input.btnWrite = false; ui.write.classList.remove('is-down'); };
  ui.write.addEventListener('pointerup', releaseWrite);
  ui.write.addEventListener('pointercancel', releaseWrite);
  ui.write.addEventListener('pointerleave', releaseWrite);

  ui.mute.addEventListener('click', () => toggleMute());
  ui.pause.addEventListener('click', () => (state === 'play' ? pauseGame() : resumeGame()));
  $('btn-start').addEventListener('click', () => { Sound.init(); startGame(); });
  $('btn-again').addEventListener('click', () => startGame());
  $('btn-resume').addEventListener('click', () => resumeGame());
  $('btn-quit').addEventListener('click', () => toTitle());

  document.addEventListener('visibilitychange', () => { if (document.hidden && state === 'play') pauseGame(); });
  window.addEventListener('blur', () => { if (state === 'play') pauseGame(); keys.clear(); input.keyWrite = input.mouseWrite = false; releaseWrite(); });

  function toggleMute() {
    Sound.muted = !Sound.muted;
    store.set('devstopian.muted', Sound.muted);
    ui.mute.classList.toggle('is-off', Sound.muted);
  }
  ui.mute.classList.toggle('is-off', Sound.muted);

  // ---------- screens ----------
  function show(screen) {
    for (const s of [ui.title, ui.pauseScreen, ui.over]) s.hidden = s !== screen;
    const inGame = state === 'play' || state === 'pause';
    ui.hud.hidden = !inGame;
    ui.write.hidden = !(coarsePointer && state === 'play');
    document.body.classList.toggle('playing', state === 'play');
    const focusable = screen && screen.querySelector('button');
    if (focusable && !coarsePointer) focusable.focus({ preventScroll: true });
  }

  let bannerTimer = 0;
  function banner(big, small, ms = 2600, note = null) {
    ui.banner.innerHTML = '';
    const b = document.createElement('span'); b.className = 'big'; b.textContent = big;
    const s = document.createElement('span'); s.className = 'small'; s.textContent = small || '';
    ui.banner.append(b, s);
    if (note) { const n = document.createElement('span'); n.className = 'note'; n.textContent = note; ui.banner.append(n); }
    ui.banner.classList.add('show');
    clearTimeout(bannerTimer);
    bannerTimer = setTimeout(() => ui.banner.classList.remove('show'), ms);
  }

  function toTitle() {
    state = 'title';
    G = newGame(true);
    ui.banner.classList.remove('show');
    $('title-best').textContent = best ? `Best: ${best.toLocaleString()} shipped` : '';
    show(ui.title);
  }

  function startGame() {
    G = newGame(false);
    state = 'play';
    Object.assign(input, { target: null, touch: null, mouseWrite: false, btnWrite: false });
    ui.notch.style.left = `${sharpAt()}%`;
    show(null);
    banner(String(G.year), YEAR_LINES[G.year]);
    Sound.year();
  }

  function pauseGame() {
    state = 'pause';
    show(ui.pauseScreen);
    ui.pause.textContent = '▶';
  }

  function resumeGame() {
    state = 'play';
    show(null);
    ui.pause.textContent = '❚❚';
  }

  // Each new year grants a random perk, with no pause in play.
  function advanceYear() {
    const pool = PERKS.filter(p => perk(p.id) < p.max);
    const p = pool.length ? pick(pool) : null;
    if (p) {
      G.perks[p.id] = perk(p.id) + 1;
      if (p.id === 'docker') { G.maxTrust += 1; G.trust = Math.min(G.maxTrust, G.trust + 1); }
    }
    G.year += 1;
    G.yearT = 0;
    G.shield = perk('gateway') ? 1 : 0;
    ui.notch.style.left = `${sharpAt()}%`;
    // A fresh year clears lingering hallucinations.
    for (const h of G.halls) burst(h.x, h.y, C.nebula, 14, 160);
    G.halls = [];
    G.timers.hall = 3;
    banner(String(G.year), YEAR_LINES[G.year] || pick(FUTURE_LINES), 3600,
      p && `+ ${p.name}${perk(p.id) > 1 ? ' II' : ''}: ${p.desc}`);
    Sound.year();
  }

  function gameOver(reason) {
    state = 'over';
    Sound.over();
    for (let i = 0; i < 3; i++) burst(G.player.x, G.player.y, reason === 'smooth' ? C.haze : C.nebula, 30, 260);
    const isBest = G.score > best;
    if (isBest) { best = G.score; store.set('devstopian.best', best); }
    $('over-eyebrow').textContent = `Game over · ${G.year}`;
    $('over-title').textContent = reason === 'smooth' ? 'You went smooth.' : 'Trust exhausted.';
    $('over-lead').textContent = reason === 'smooth'
      ? 'Your output became the statistically average response. The edge is gone.'
      : 'Too many hallucinations shipped to prod. Nobody believes the build anymore.';
    const stats = [
      ['Shipped', G.score.toLocaleString()],
      ['Reached', G.year],
      ['Tokens consumed', G.stats.tokens],
      ['Rough ideas', G.stats.shards],
      ['Hallucinations caught', G.stats.caught],
      ['Time writing', `${Math.round(G.stats.writeTime)}s`],
    ];
    const dl = $('over-stats');
    dl.innerHTML = '';
    for (const [k, v] of stats) {
      const d = document.createElement('div');
      const dt = document.createElement('dt'); dt.textContent = k;
      const dd = document.createElement('dd'); dd.textContent = v;
      d.append(dt, dd);
      dl.append(d);
    }
    $('over-best').textContent = isBest ? 'New best!' : `Best: ${best.toLocaleString()}`;
    // Let the final burst play before the panel covers it.
    ui.hud.hidden = true;
    ui.write.hidden = true;
    setTimeout(() => { if (state === 'over') show(ui.over); }, 900);
  }

  // ---------- spawning ----------
  function edgePoint(m = 30) {
    switch (Math.floor(Math.random() * 4)) {
      case 0: return { x: rand(0, W), y: -m };
      case 1: return { x: W + m, y: rand(0, H) };
      case 2: return { x: rand(0, W), y: H + m };
      default: return { x: -m, y: rand(0, H) };
    }
  }

  function spawnTokens() {
    const L = G.attract ? 1 : lvl();
    const from = edgePoint(30);
    const to = { x: rand(W * 0.2, W * 0.8), y: rand(H * 0.2, H * 0.8) };
    const d = Math.hypot(to.x - from.x, to.y - from.y) || 1;
    const dx = (to.x - from.x) / d, dy = (to.y - from.y) / d;
    const speed = rand(70, 140) + L * 14;
    const stream = Math.random() < 0.22 + L * 0.05 ? 3 + Math.floor(Math.random() * (2 + L)) : 1;
    for (let i = 0; i < stream; i++) {
      G.tokens.push({
        x: from.x - dx * 26 * i, y: from.y - dy * 26 * i,
        vx: dx * speed, vy: dy * speed, r: 10, glyph: pick(TOKEN_GLYPHS), age: 0,
      });
    }
  }

  function spawnShard() {
    let x, y, tries = 0;
    do {
      x = rand(60, W - 60); y = rand(90, H - 60); tries++;
    } while (tries < 10 && dist2({ x, y }, G.player) < 180 * 180);
    G.shards.push({ x, y, vx: rand(-25, 25), vy: rand(-25, 25), r: 12, rot: rand(0, 6.28), spin: rand(-2, 2), age: 0, life: 9 });
  }

  function spawnHall() {
    const p = edgePoint(40);
    G.halls.push({ x: p.x, y: p.y, vx: 0, vy: 0, r: 16, age: 0, life: 16, seed: rand(0, 100), glyph: pick(TOKEN_GLYPHS) });
  }

  // ---------- effects ----------
  function burst(x, y, color, n, speed) {
    const count = reducedMotion ? Math.ceil(n / 3) : n;
    for (let i = 0; i < count; i++) {
      const a = rand(0, Math.PI * 2), s = rand(speed * 0.2, speed);
      G.parts.push({ x, y, vx: Math.cos(a) * s, vy: Math.sin(a) * s, life: rand(0.35, 0.8), age: 0, color, size: rand(1.5, 3.2) });
    }
  }

  function floatText(x, y, text, color) {
    G.texts.push({ x, y, text, color, age: 0, life: 0.9 });
  }

  function shake(amount) {
    if (!reducedMotion) G.shake = Math.max(G.shake, amount);
  }

  // ---------- update ----------
  function updatePlay(dt) {
    const p = G.player;
    G.t += dt;
    G.yearT += dt;
    const L = lvl();

    // --- writing ---
    G.writing = input.keyWrite || input.mouseWrite || input.btnWrite;
    if (G.writing) {
      G.edge = Math.min(EDGE_MAX, G.edge + 13 * 1.5 ** perk('gitflow') * dt);
      G.stats.writeTime += dt;
      G.timers.key -= dt;
      if (G.timers.key <= 0) { Sound.key(); G.timers.key = rand(0.05, 0.13); }
    }

    // --- passive decay: the flood wears you down ---
    G.edge -= (0.5 + L * 0.18) * (perk('stopvibe') ? 0.5 : 1) * dt;

    // --- movement ---
    const maxSpeed = 380 * 1.15 ** perk('rust') * (G.writing ? (perk('promptme') ? 0.78 : 0.42) : 1);
    let dx = 0, dy = 0;
    for (const k of keys) { const d = DIR_KEYS[k]; if (d) { dx += d[0]; dy += d[1]; } }
    let tvx = 0, tvy = 0;
    if (dx || dy) {
      const m = Math.hypot(dx, dy);
      tvx = (dx / m) * maxSpeed; tvy = (dy / m) * maxSpeed;
    } else {
      let target = input.target;
      if (input.touch && input.touch.fx2 !== undefined) {
        const t = input.touch;
        target = { x: t.sx + (t.fx2 - t.fx) * 1.5, y: t.sy + (t.fy2 - t.fy) * 1.5 };
      }
      if (target) {
        tvx = (target.x - p.x) * 8; tvy = (target.y - p.y) * 8;
        const m = Math.hypot(tvx, tvy);
        if (m > maxSpeed) { tvx = (tvx / m) * maxSpeed; tvy = (tvy / m) * maxSpeed; }
      }
    }
    const k = Math.min(1, dt * 12);
    p.vx += (tvx - p.vx) * k;
    p.vy += (tvy - p.vy) * k;
    p.x = clamp(p.x + p.vx * dt, 10, W - 10);
    p.y = clamp(p.y + p.vy * dt, 10, H - 10);
    if (Math.hypot(p.vx, p.vy) > 30) {
      const target = Math.atan2(p.vy, p.vx);
      let diff = target - p.angle;
      diff = Math.atan2(Math.sin(diff), Math.cos(diff));
      p.angle += diff * Math.min(1, dt * 10);
    }
    if (p.inv > 0) p.inv -= dt;

    // thruster sparks
    if (Math.hypot(p.vx, p.vy) > 120 && Math.random() < 0.6) {
      const a = p.angle + Math.PI + rand(-0.3, 0.3);
      G.parts.push({ x: p.x - Math.cos(p.angle) * 10, y: p.y - Math.sin(p.angle) * 10, vx: Math.cos(a) * 90, vy: Math.sin(a) * 90, life: 0.3, age: 0, color: G.writing ? C.signal : C.plasma, size: 2 });
    }

    // --- spawns ---
    G.timers.token -= dt;
    if (G.timers.token <= 0) { spawnTokens(); G.timers.token = rand(0.6, 1.4) / (1.3 + L * 0.55); }
    G.timers.shard -= dt;
    if (G.timers.shard <= 0) { spawnShard(); G.timers.shard = rand(2.4, 4.0) / (1 + 0.4 * perk('artifact')); }
    G.timers.hall -= dt;
    if (G.timers.hall <= 0) {
      if (G.halls.length < 2 + L * 2) spawnHall();
      G.timers.hall = Math.max(1.1, 5.5 - L * 0.8) * rand(0.8, 1.2);
    }

    // --- tokens ---
    const magnet = perk('agentic') ? 70 + 50 * perk('agentic') : 0;
    const drain = (2.4 + L * 0.15) * 0.75 ** perk('immutability');
    for (let i = G.tokens.length - 1; i >= 0; i--) {
      const t = G.tokens[i];
      t.age += dt;
      const d2 = dist2(t, p);
      if (G.writing && d2 < 70 * 70) {
        // Writing pushes the flood away.
        const d = Math.sqrt(d2) || 1;
        t.vx += ((t.x - p.x) / d) * 600 * dt;
        t.vy += ((t.y - p.y) / d) * 600 * dt;
      } else if (magnet && d2 < magnet * magnet) {
        const d = Math.sqrt(d2) || 1;
        t.vx += ((p.x - t.x) / d) * 700 * dt;
        t.vy += ((p.y - t.y) / d) * 700 * dt;
      }
      t.x += t.vx * dt; t.y += t.vy * dt;
      if (!G.writing && d2 < (t.r + p.r) ** 2) {
        const gain = Math.round(10 * multiplier());
        G.score += gain;
        G.edge -= drain;
        G.stats.tokens++;
        burst(t.x, t.y, C.plasma, 6, 120);
        floatText(t.x, t.y - 10, `+${gain}`, C.plasmaLight);
        Sound.token(G.edge);
        G.tokens.splice(i, 1);
        continue;
      }
      if (t.age > 25 || t.x < -120 || t.x > W + 120 || t.y < -120 || t.y > H + 120) G.tokens.splice(i, 1);
    }

    // --- shards (rough ideas) ---
    for (let i = G.shards.length - 1; i >= 0; i--) {
      const s = G.shards[i];
      s.age += dt;
      s.vx += rand(-60, 60) * dt; s.vy += rand(-60, 60) * dt;
      s.vx = clamp(s.vx, -40, 40); s.vy = clamp(s.vy, -40, 40);
      s.x = clamp(s.x + s.vx * dt, 20, W - 20);
      s.y = clamp(s.y + s.vy * dt, 20, H - 20);
      s.rot += s.spin * dt;
      if (dist2(s, p) < (s.r + p.r + 4) ** 2) {
        const gain = 20 * 1.5 ** perk('techstack');
        G.edge = Math.min(EDGE_MAX, G.edge + gain);
        G.score += 25;
        G.stats.shards++;
        burst(s.x, s.y, C.signal, 16, 200);
        floatText(s.x, s.y - 12, 'rough idea', C.signalLight);
        Sound.shard();
        G.shards.splice(i, 1);
        continue;
      }
      if (s.age > s.life) G.shards.splice(i, 1);
    }

    // --- hallucinations ---
    const hallSpeed = (55 + L * 12) * 0.75 ** perk('testgen');
    for (let i = G.halls.length - 1; i >= 0; i--) {
      const h = G.halls[i];
      h.age += dt;
      const d = Math.sqrt(dist2(h, p)) || 1;
      const wob = Math.sin(G.t * 2 + h.seed) * 0.6;
      const ax = (p.x - h.x) / d, ay = (p.y - h.y) / d;
      const tx = (ax * Math.cos(wob) - ay * Math.sin(wob)) * hallSpeed;
      const ty = (ax * Math.sin(wob) + ay * Math.cos(wob)) * hallSpeed;
      h.vx += (tx - h.vx) * Math.min(1, dt * 1.5);
      h.vy += (ty - h.vy) * Math.min(1, dt * 1.5);
      h.x += h.vx * dt; h.y += h.vy * dt;

      if (h.age > 0.6 && d < h.r + p.r - 2 && p.inv <= 0) {
        if (isSharp()) {
          G.edge -= 10;
          const gain = Math.round(40 * multiplier());
          G.score += gain;
          G.stats.caught++;
          burst(h.x, h.y, C.nebula, 26, 260);
          floatText(h.x, h.y - 14, `caught +${gain}`, C.nebula);
          Sound.caught();
          shake(4);
        } else if (G.shield > 0) {
          G.shield--;
          p.inv = 1;
          burst(h.x, h.y, C.signal, 20, 220);
          floatText(p.x, p.y - 20, 'blocked', C.signalLight);
          Sound.block();
        } else {
          G.trust--;
          G.stats.hits++;
          p.inv = 1.6;
          burst(p.x, p.y, C.neg, 24, 240);
          floatText(p.x, p.y - 20, 'fooled −trust', C.neg);
          Sound.hit();
          shake(12);
        }
        G.halls.splice(i, 1);
        if (G.trust <= 0) return gameOver('trust');
        continue;
      }
      if (h.age > h.life) { burst(h.x, h.y, C.quasar, 8, 80); G.halls.splice(i, 1); }
    }

    // --- edge bottoming out ---
    if (G.edge <= 0) {
      if (perk('mvdb') && !G.mvdbUsed) {
        G.mvdbUsed = true;
        G.edge = 40;
        banner('Rolled back', 'Multi-Version DB restored an earlier you.', 2000);
        Sound.block();
      } else {
        G.edge = 0;
        return gameOver('smooth');
      }
    }

    if (G.yearT >= YEAR_SECONDS) advanceYear();
  }

  function updateAttract(dt) {
    G.t += dt;
    G.timers.token -= dt;
    if (G.timers.token <= 0) { spawnTokens(); G.timers.token = rand(0.5, 1.2); }
    for (let i = G.tokens.length - 1; i >= 0; i--) {
      const t = G.tokens[i];
      t.age += dt; t.x += t.vx * dt; t.y += t.vy * dt;
      if (t.age > 25 || t.x < -120 || t.x > W + 120 || t.y < -120 || t.y > H + 120) G.tokens.splice(i, 1);
    }
  }

  function updateFx(dt) {
    for (let i = G.parts.length - 1; i >= 0; i--) {
      const q = G.parts[i];
      q.age += dt;
      q.x += q.vx * dt; q.y += q.vy * dt;
      q.vx *= 1 - dt * 3; q.vy *= 1 - dt * 3;
      if (q.age > q.life) G.parts.splice(i, 1);
    }
    for (let i = G.texts.length - 1; i >= 0; i--) {
      const t = G.texts[i];
      t.age += dt; t.y -= 34 * dt;
      if (t.age > t.life) G.texts.splice(i, 1);
    }
    G.shake = Math.max(0, G.shake - dt * 30);
  }

  // ---------- render ----------
  function drawTokens() {
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    ctx.font = '700 9px "JetBrains Mono", ui-monospace, monospace';
    for (const t of G.tokens) {
      ctx.fillStyle = 'rgba(56,217,224,0.14)';
      ctx.beginPath(); ctx.arc(t.x, t.y, t.r + 6, 0, Math.PI * 2); ctx.fill();
      ctx.fillStyle = C.plasma;
      ctx.beginPath(); ctx.arc(t.x, t.y, t.r, 0, Math.PI * 2); ctx.fill();
      ctx.fillStyle = C.void;
      ctx.fillText(t.glyph, t.x, t.y + 0.5);
    }
  }

  function drawShards() {
    for (const s of G.shards) {
      const fadeIn = Math.min(1, s.age * 3);
      const ending = s.life - s.age < 2 ? (Math.sin(s.age * 20) > 0 ? 1 : 0.35) : 1;
      ctx.save();
      ctx.globalAlpha = fadeIn * ending;
      ctx.translate(s.x, s.y);
      ctx.rotate(s.rot);
      ctx.fillStyle = 'rgba(242,167,58,0.16)';
      ctx.beginPath(); ctx.arc(0, 0, s.r + 9, 0, Math.PI * 2); ctx.fill();
      ctx.beginPath();
      const pts = 7;
      for (let i = 0; i < pts * 2; i++) {
        const a = (i / (pts * 2)) * Math.PI * 2;
        const r = i % 2 ? s.r * (0.38 + 0.12 * Math.sin(i * 3.7)) : s.r * (0.9 + 0.2 * Math.cos(i * 2.3));
        ctx.lineTo(Math.cos(a) * r, Math.sin(a) * r);
      }
      ctx.closePath();
      ctx.fillStyle = C.signal;
      ctx.fill();
      ctx.strokeStyle = C.signalLight;
      ctx.lineWidth = 1;
      ctx.stroke();
      ctx.restore();
    }
  }

  function drawHalls() {
    // When your edge is low, hallucinations look like real output.
    const disguise = G.attract ? 0 : clamp((sharpAt() - G.edge) / sharpAt(), 0, 0.85);
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    for (const h of G.halls) {
      const fadeIn = Math.min(1, h.age * 2);
      const fadeOut = Math.min(1, (h.life - h.age) * 1.5);
      ctx.save();
      ctx.globalAlpha = Math.max(0, fadeIn * fadeOut);
      ctx.translate(h.x, h.y);
      const r = h.r * (1 - disguise * 0.35);
      const wobAmp = 0.18 * (1 - disguise);
      const glow = ctx.createRadialGradient(0, 0, 0, 0, 0, r * 2);
      glow.addColorStop(0, mix(C.nebula, C.plasma, disguise, 0.35));
      glow.addColorStop(1, mix(C.quasar, C.plasma, disguise, 0));
      ctx.fillStyle = glow;
      ctx.beginPath(); ctx.arc(0, 0, r * 2, 0, Math.PI * 2); ctx.fill();
      ctx.beginPath();
      const n = 18;
      for (let i = 0; i <= n; i++) {
        const a = (i / n) * Math.PI * 2;
        const rr = r * (1 + wobAmp * Math.sin(a * 3 + G.t * 4 + h.seed) + wobAmp * 0.6 * Math.cos(a * 5 - G.t * 3));
        ctx.lineTo(Math.cos(a) * rr, Math.sin(a) * rr);
      }
      ctx.closePath();
      ctx.fillStyle = mix(C.nebula, C.plasma, disguise);
      ctx.fill();
      ctx.fillStyle = disguise > 0.5 ? C.void : C.star;
      ctx.font = `700 ${disguise > 0.5 ? 9 : 14}px "JetBrains Mono", ui-monospace, monospace`;
      ctx.fillText(disguise > 0.5 ? h.glyph : '?', 0, 1);
      ctx.restore();
    }
  }

  function drawPlayer() {
    const p = G.player;
    const e = G.edge / EDGE_MAX;
    const sharp = isSharp();
    ctx.save();
    ctx.translate(p.x, p.y);

    if (G.writing) {
      ctx.save();
      ctx.rotate(G.t * 1.5);
      ctx.strokeStyle = C.signal;
      ctx.globalAlpha = 0.75;
      ctx.lineWidth = 2;
      ctx.setLineDash([6, 7]);
      ctx.beginPath(); ctx.arc(0, 0, 30 + Math.sin(G.t * 8) * 2, 0, Math.PI * 2); ctx.stroke();
      ctx.globalAlpha = 0.18;
      ctx.setLineDash([]);
      ctx.beginPath(); ctx.arc(0, 0, 70, 0, Math.PI * 2); ctx.stroke();
      ctx.restore();
    }
    if (G.shield > 0) {
      ctx.strokeStyle = 'rgba(242,167,58,0.5)';
      ctx.lineWidth = 1.5;
      ctx.beginPath(); ctx.arc(0, 0, 22, 0, Math.PI * 2); ctx.stroke();
    }

    ctx.rotate(p.angle);
    if (p.inv > 0 && Math.floor(p.inv * 12) % 2) ctx.globalAlpha = 0.3;

    // Low edge: soft glow and no spikes. High edge: a rough, jagged hull.
    ctx.shadowColor = C.plasma;
    ctx.shadowBlur = (1 - e) * 26;
    const amp = e * 4.5;
    ctx.beginPath();
    const n = SHIP_OUTLINE.length;
    for (let i = 0; i < n; i++) {
      const [x1, y1] = SHIP_OUTLINE[i];
      const [x2, y2] = SHIP_OUTLINE[(i + 1) % n];
      const nx = -(y2 - y1), ny = x2 - x1;
      const nl = Math.hypot(nx, ny) || 1;
      for (let j = 0; j < 3; j++) {
        const f = j / 3;
        const off = j === 0 ? 0 : SHIP_NOISE[i * 3 + j] * amp;
        ctx.lineTo(x1 + (x2 - x1) * f + (nx / nl) * off, y1 + (y2 - y1) * f + (ny / nl) * off);
      }
    }
    ctx.closePath();
    ctx.lineJoin = e < 0.3 ? 'round' : 'miter';
    ctx.fillStyle = mix('#8e97a9', '#1a2236', e);
    ctx.fill();
    ctx.shadowBlur = 0;
    ctx.lineWidth = 2;
    ctx.strokeStyle = sharp ? C.signal : mix('#8e97a9', C.plasma, e * 1.5 > 1 ? 1 : e * 1.5);
    ctx.stroke();
    // cockpit
    ctx.fillStyle = sharp ? C.signalLight : C.plasmaLight;
    ctx.beginPath(); ctx.arc(4, 0, 2.6, 0, Math.PI * 2); ctx.fill();
    ctx.restore();
  }

  function drawFx() {
    for (const q of G.parts) {
      ctx.globalAlpha = 1 - q.age / q.life;
      ctx.fillStyle = q.color;
      ctx.fillRect(q.x - q.size / 2, q.y - q.size / 2, q.size, q.size);
    }
    ctx.globalAlpha = 1;
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    ctx.font = '700 12px "JetBrains Mono", ui-monospace, monospace';
    for (const t of G.texts) {
      ctx.globalAlpha = 1 - t.age / t.life;
      ctx.fillStyle = t.color;
      ctx.fillText(t.text, t.x, t.y);
    }
    ctx.globalAlpha = 1;
  }

  function render() {
    ctx.save();
    if (G.shake > 0) ctx.translate(rand(-G.shake, G.shake), rand(-G.shake, G.shake));
    drawBackground(G.t);
    drawShards();
    drawTokens();
    drawHalls();
    if (!G.attract && state !== 'over') drawPlayer();
    drawFx();
    ctx.restore();
  }

  // ---------- HUD ----------
  const hudCache = {};
  function setText(el, key, v) { if (hudCache[key] !== v) { hudCache[key] = v; el.textContent = v; } }

  function updateHud() {
    if (G.attract) return;
    setText(ui.year, 'year', String(G.year));
    setText(ui.score, 'score', G.score.toLocaleString());
    setText(ui.mult, 'mult', `×${multiplier().toFixed(1)}`);
    ui.yearBar.style.width = `${(G.yearT / YEAR_SECONDS) * 100}%`;
    ui.edgeFill.style.width = `${clamp(G.edge, 0, EDGE_MAX)}%`;
    const sharp = isSharp();
    ui.edge.classList.toggle('is-dull', !sharp);
    ui.edge.classList.toggle('is-critical', G.edge < 20);
    setText(ui.edgeState, 'edgeState', G.writing ? 'write' : sharp ? 'sharp' : G.edge < 20 ? 'smooth' : 'dull');
    const trustKey = `${G.trust}/${G.maxTrust}/${G.shield}`;
    if (hudCache.trust !== trustKey) {
      hudCache.trust = trustKey;
      ui.trust.innerHTML = '';
      for (let i = 0; i < G.maxTrust; i++) {
        const d = document.createElement('i');
        if (i < G.trust) d.className = 'on';
        ui.trust.append(d);
      }
      if (G.shield) { const s = document.createElement('i'); s.className = 'shield'; ui.trust.append(s); }
      ui.trust.setAttribute('aria-label', `Trust ${G.trust} of ${G.maxTrust}`);
    }
  }

  // ---------- loop ----------
  let last = performance.now();
  function frame(now) {
    const dt = Math.min(0.05, (now - last) / 1000);
    last = now;
    if (state !== 'pause') updateStars(dt);
    if (state === 'play') updatePlay(dt);
    else if (state === 'title') updateAttract(dt);
    if (state === 'play' || state === 'title' || state === 'over') updateFx(dt);
    render();
    updateHud();
    requestAnimationFrame(frame);
  }

  // Test hook: lets automated checks drive the game without real input.
  window.__devstopian = { get state() { return state; }, get game() { return G; }, startGame, advanceYear };

  window.addEventListener('resize', resize);
  resize();
  toTitle();
  requestAnimationFrame(frame);
})();
