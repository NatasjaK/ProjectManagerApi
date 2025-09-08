import { useEffect, useMemo, useState } from "react";
import { Projects } from "./api";
import { Uploads } from "./api";
import "./app.css";

export default function App() {
  const [items, setItems] = useState([]);
  const [sort, setSort] = useState("desc");          
  const [completedOnly, setCompletedOnly] = useState(false);
  const [loading, setLoading] = useState(false);
  const [err, setErr] = useState("");

  const [showAdd, setShowAdd] = useState(false);
  const [editItem, setEditItem] = useState(null);     
  const [confirmDelete, setConfirmDelete] = useState(null);
  const [menuOpenId, setMenuOpenId] = useState(null); 

  const reload = async () => {
    setLoading(true); setErr("");
    try {
      const data = await Projects.list({ sort, completedOnly });
      setItems(data);
    } catch (e) {
      setErr(e?.response?.data ?? e?.message ?? "Unknown error");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { reload(); }, [sort, completedOnly]);

  const allCount = items.length;
  const completedCount = items.filter(p => p.isCompleted).length;
  const list = useMemo(() => items, [items]);

  return (
    <div className="page">
      {}
      <header className="topbar">
        <div className="brand">
          <div className="logo-dot" /> <span className="brand-name">alpha</span>
        </div>
        <div className="spacer" />
      </header>

      <main className="content">
        <div className="content-head">
          <h1>Projects</h1>
          <button className="btn-primary" onClick={() => setShowAdd(true)}>+ Add Project</button>
        </div>

        {/* Tabs */}
        <nav className="tabs">
          <button
            className={`tab ${!completedOnly ? "active" : ""}`}
            onClick={() => setCompletedOnly(false)}
            aria-current={!completedOnly ? "page" : undefined}
          >
            ALL <span className="count">({allCount})</span>
          </button>
          <button
            className={`tab ${completedOnly ? "active" : ""}`}
            onClick={() => setCompletedOnly(true)}
            aria-current={completedOnly ? "page" : undefined}
          >
            COMPLETED <span className="count">({completedCount})</span>
          </button>
          <div className="spacer" />
          {}
          <select
            className="select sort"
            value={sort}
            onChange={e => setSort(e.target.value)}
            title="Sort order"
          >
            <option value="desc">Newest</option>
            <option value="asc">Oldest</option>
          </select>
        </nav>

        {err && <div className="alert">{err}</div>}
        {loading ? <p>Loading…</p> : (
          <ul className="card-grid">
            {list.map(p => (
              <li key={p.id} className="card">
                {}
                <div className="card-top">
                  <div className="avatar" style={{ background: colorFor(p.title) }}>
                    {p.title?.[0]?.toUpperCase() ?? "P"}
                  </div>
                  <button
                    className="kebab"
                    aria-label="More options"
                    onClick={() => setMenuOpenId(menuOpenId === p.id ? null : p.id)}
                  >
                    ⋮
                  </button>
                  {menuOpenId === p.id && (
                    <div className="menu" onMouseLeave={() => setMenuOpenId(null)}>
                      <button onClick={() => { setEditItem(p); setMenuOpenId(null); }}>Edit</button>
                      <button className="danger" onClick={() => { setConfirmDelete(p); setMenuOpenId(null); }}>Delete Project</button>
                    </div>
                  )}
                </div>

                <h3 className="card-title">{p.title}</h3>
                {p.description && <p className="muted">{p.description}</p>}
                {p.clientName && <p className="muted">Client: {p.clientName}</p>}
                {p.ownerName && <p className="muted">Owner: {p.ownerName}</p>}
                {typeof p.budget === "number" && <p className="muted">
  Budget: {p.budget.toLocaleString()} {p.currency ?? ""}</p>}  

                <div className="card-foot">
                  <span className={`badge ${p.isCompleted ? "green" : "gray"}`}>
                    {p.isCompleted ? "COMPLETED" : "OPEN"}
                  </span>
                  <div className="spacer" />
                  <small className="muted">{p.createdAtUtc ? new Date(p.createdAtUtc).toLocaleDateString() : ""}</small>
                </div>
              </li>
            ))}
          </ul>
        )}
      </main>

      {}
      {showAdd && (
        <ProjectModal
          title="Add Project"
          onClose={() => setShowAdd(false)}
          onSave={async (data) => {
            try { await Projects.create(data); setShowAdd(false); reload(); }
            catch (e) { alert(e?.response?.data ?? e?.message ?? "Error"); }
          }}
        />
      )}

      {}
      {editItem && (
        <ProjectModal
          title="Edit Project"
          initial={editItem}
          onClose={() => setEditItem(null)}
          onSave={async (data) => {
            try { await Projects.update(editItem.id, data); setEditItem(null); reload(); }
            catch (e) { alert(e?.response?.data ?? e?.message ?? "Error"); }
          }}
        />
      )}

      {}
      {confirmDelete && (
        <ConfirmModal
          text={`Delete "${confirmDelete.title}"?`}
          onCancel={() => setConfirmDelete(null)}
          onConfirm={async () => {
            try { await Projects.remove(confirmDelete.id); }
            finally { setConfirmDelete(null); reload(); }
          }}
        />
      )}
    </div>
  );
}

function ProjectModal({ title, initial, onClose, onSave }) {
  const [clients, setClients] = useState(["Acme AB", "Globex", "Initech"]);
  const [owners, setOwners]   = useState(["Anna Nilsson", "Erik Svensson", "Sara Lund"]);

  const [form, setForm] = useState({
    title: initial?.title ?? "",
    description: initial?.description ?? "",
    imageUrl: initial?.imageUrl ?? "",
    isCompleted: initial?.isCompleted ?? false,

    clientName: initial?.clientName ?? "",
    ownerName:  initial?.ownerName  ?? "",
    startDateUtc: initial?.startDateUtc ? toLocalInputValue(initial.startDateUtc) : "",
    dueDateUtc:   initial?.dueDateUtc   ? toLocalInputValue(initial.dueDateUtc)   : "",
    budget: initial?.budget ?? "",
    currency: initial?.currency ?? "SEK",
  });

  const onChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm(f => ({ ...f, [name]: type === "checkbox" ? checked : value }));
  };

  const submit = (e) => {
    e.preventDefault();
    const payload = { ...form };

    if (!payload.imageUrl) delete payload.imageUrl;
    if (!payload.clientName) delete payload.clientName;
    if (!payload.ownerName)  delete payload.ownerName;

    if (payload.startDateUtc) payload.startDateUtc = new Date(payload.startDateUtc).toISOString();
    else delete payload.startDateUtc;

    if (payload.dueDateUtc) payload.dueDateUtc = new Date(payload.dueDateUtc).toISOString();
    else delete payload.dueDateUtc;

    if (payload.budget !== "" && payload.budget !== null && payload.budget !== undefined) {
      payload.budget = Number(payload.budget);
    } else {
      delete payload.budget;
    }

    if (!payload.currency) delete payload.currency;

    onSave(payload);
  };

  return (
    <div className="modal-backdrop" onClick={onClose}>
      <div className="modal" onClick={e => e.stopPropagation()}>
        <h2>{title}</h2>

        <ImagePicker
          url={form.imageUrl}
          onChange={(url) => setForm(f => ({ ...f, imageUrl: url }))}
        />

        <form onSubmit={submit} className="form">
          <label>Project Name
            <input name="title" value={form.title} onChange={onChange} required maxLength={200} />
          </label>

          <SelectWithMenu
            label="Client Name"
            name="clientName"
            value={form.clientName}
            options={clients}
            onChange={onChange}
            onAddNew={() => {
              const v = prompt("Add new client name:");
              if (v && !clients.includes(v)) setClients(s => [...s, v]);
              if (v) setForm(f => ({ ...f, clientName: v }));
            }}
            onClear={() => setForm(f => ({ ...f, clientName: "" }))}
          />

          <SelectWithMenu
            label="Project Owner"
            name="ownerName"
            value={form.ownerName}
            options={owners}
            onChange={onChange}
            onAddNew={() => {
              const v = prompt("Add new project owner:");
              if (v && !owners.includes(v)) setOwners(s => [...s, v]);
              if (v) setForm(f => ({ ...f, ownerName: v }));
            }}
            onClear={() => setForm(f => ({ ...f, ownerName: "" }))}
          />

          <label>Description
            <textarea name="description" value={form.description} onChange={onChange} maxLength={2000} />
          </label>

          <div className="row">
            <label>Start Date
              <input name="startDateUtc" type="datetime-local" value={form.startDateUtc} onChange={onChange} />
            </label>
            <label>End Date
              <input name="dueDateUtc" type="datetime-local" value={form.dueDateUtc} onChange={onChange} />
            </label>
          </div>

          <div className="row">
            <label className="field">Budget
              <input
                name="budget"
                type="number"
                step="any"
                value={form.budget}
                onChange={onChange}
                inputMode="decimal"
                placeholder="0.0"
              />
            </label>

            <label className="field">Currency
              <select name="currency" value={form.currency} onChange={onChange}>
                <option>SEK</option>
                <option>EUR</option>
                <option>USD</option>
                <option>NOK</option>
                <option>DKK</option>
              </select>
            </label>
          </div>

          <label className="row">
            <input type="checkbox" name="isCompleted" checked={form.isCompleted} onChange={onChange} />
            Completed
          </label>

          <div className="row end">
            <button type="button" onClick={onClose}>Cancel</button>
            <button className="btn-primary" type="submit">Save</button>
          </div>
        </form>
      </div>
    </div>
  );
}

function ImagePicker({ url, onChange }) {
  const inputId = useMemo(() => "file-" + Math.random().toString(36).slice(2), []);
  const [dragOver, setDragOver] = useState(false);

  const uploadFile = async (file) => {
    if (!file) return;
    try {
      const uploadedUrl = await Uploads.image(file);  // POST /api/uploads/image
      onChange(uploadedUrl);                           
    } catch (e) {
      alert(e?.response?.data ?? e?.message ?? "Upload failed");
    }
  };

  return (
    <div
      className={`image-header ${url ? "has-img" : ""} ${dragOver ? "drag" : ""}`}
      onDragOver={(e) => { e.preventDefault(); setDragOver(true); }}
      onDragLeave={() => setDragOver(false)}
      onDrop={(e) => { e.preventDefault(); setDragOver(false); uploadFile(e.dataTransfer.files?.[0]); }}
    >
      {url ? <img src={url} alt="project" /> : <div className="hint">Click or drop to upload</div>}

      {/* Välj fil */}
      <label htmlFor={inputId} className="image-overlay"><span>Upload</span></label>
      <input
        id={inputId}
        type="file"
        accept="image/*"
        style={{ display: "none" }}
        onChange={(e) => uploadFile(e.target.files?.[0])}
      />

      {/* Alternativ: klistra in URL */}
      <button
        type="button"
        className="image-url-btn"
        onClick={() => {
          const v = prompt("Paste image URL:", url || "");
          if (v != null) onChange(v.trim());
        }}
      >
        URL
      </button>
    </div>
  );
}


function SelectWithMenu({ label, name, value, options, onChange, onAddNew, onClear }) {
  const [open, setOpen] = useState(false);
  return (
    <div className="field">
      <div className="input-row">
        <label style={{ flex: 1 }}>{label}
          <select name={name} value={value} onChange={onChange} style={{ width: "100%" }}>
            <option value="">— Select —</option>
            {options.map(o => <option key={o} value={o}>{o}</option>)}
          </select>
        </label>
        <div className="pop">
          <button type="button" className="dots" onClick={() => setOpen(o => !o)} aria-label="More">
            ⋮
          </button>
          {open && (
            <div className="popmenu" onMouseLeave={() => setOpen(false)}>
              <button type="button" onClick={() => { setOpen(false); onAddNew?.(); }}>Add new…</button>
              <button type="button" onClick={() => { setOpen(false); onClear?.(); }}>Clear</button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}



function ConfirmModal({ text, onCancel, onConfirm }) {
  return (
    <div className="modal-backdrop" onClick={onCancel}>
      <div className="modal" onClick={e => e.stopPropagation()}>
        <p>{text}</p>
        <div className="row end">
          <button onClick={onCancel}>Cancel</button>
          <button className="danger" onClick={onConfirm}>Delete</button>
        </div>
      </div>
    </div>
  );
}

function toLocalInputValue(iso) {
  const d = new Date(iso);
  d.setMinutes(d.getMinutes() - d.getTimezoneOffset());
  return d.toISOString().slice(0,16);
}

function colorFor(title = "") {
  const palette = [
    "linear-gradient(135deg,#7AE7C7,#3CC2A3)",
    "linear-gradient(135deg,#A78BFA,#7C3AED)",
    "linear-gradient(135deg,#FCD34D,#F59E0B)",
    "linear-gradient(135deg,#60A5FA,#3B82F6)",
    "linear-gradient(135deg,#FCA5A5,#F87171)",
    "linear-gradient(135deg,#93C5FD,#60A5FA)",
    "linear-gradient(135deg,#86EFAC,#22C55E)",
    "linear-gradient(135deg,#FBB6CE,#F472B6)"
  ];
  let hash = 0;
  for (let i = 0; i < title.length; i++) hash = title.charCodeAt(i) + ((hash << 5) - hash);
  return palette[Math.abs(hash) % palette.length];
}
