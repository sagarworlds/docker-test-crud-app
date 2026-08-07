import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ConfirmDialogComponent {

  // ── Signal-based inputs (Angular 17.3+ / Angular 22) ─────
  readonly title        = input<string>('Confirm Action');
  readonly message      = input<string>('Are you sure you want to proceed?');
  readonly confirmLabel = input<string>('Confirm');
  readonly cancelLabel  = input<string>('Cancel');

  // ── Signal-based outputs ──────────────────────────────────
  readonly confirmed = output<void>();
  readonly cancelled = output<void>();

  onConfirm(): void { this.confirmed.emit(); }
  onCancel():  void { this.cancelled.emit(); }
}
